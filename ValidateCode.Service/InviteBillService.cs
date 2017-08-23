
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ValidateCode.Core.Code;
using ValidateCode.Core.Extensions;
using ValidateCode.Core.Model;
using ValidateCode.DB;
using ValidateCode.IService;
using ValidateCode.Model;
using ValidateCode.Service;

namespace ValidateCode.Service
{
    /// <summary>
    /// 用户提现
    /// </summary>
    public class InviteBillService : BaseService<invite_bill>, IInviteBillService
    {

        private  InviteService inviteService;
        private IAppUserService appUserService;


        public PageList<invite_bill> GetPageList(int pageIndex, int pageSize,string name,DateTime? createdTimeStart, DateTime? createdTimeEnd,int userId=0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<invite_bill>();
                var count = 0;
                if (userId != 0)
                {
                    var query = db.invite_bill.Where(x => x.statu == EntityStatu.normal && x.app_user_id == userId&&x.invite_state==AuditState.success);
                    if (name.IsNotNullOrEmpty())
                    {
                        var projectIdList = db.project.Where(x => x.statu == EntityStatu.normal && x.name.Contains(name)).Select(x => x.id).ToList();
                        if (projectIdList != null && projectIdList.Count > 0)
                        {
                            query = query.Where(x => projectIdList.Contains(x.project_id));
                        }
                    }
                    if (createdTimeStart != null)
                    {
                        query = query.Where(x => x.verify_time >= createdTimeStart);
                    }
                    if (createdTimeEnd != null)
                    {
                        createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                        query = query.Where(x => x.verify_time < createdTimeEnd);
                    }
                    count = query.Count();
                    list = query.OrderByDescending(x => x.verify_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    var query = db.invite_bill.Where(x => x.statu == EntityStatu.normal);
                    if (name.IsNotNullOrEmpty())
                    {
                        var projectIdList = db.project.Where(x => x.statu == EntityStatu.normal && x.name.Contains(name)).Select(x => x.id).ToList();
                        if (projectIdList != null && projectIdList.Count > 0)
                        {
                            query = query.Where(x => projectIdList.Contains(x.project_id));
                        }
                    }
                    if (createdTimeStart != null)
                    {
                        query = query.Where(x => x.verify_time >= createdTimeStart);
                    }
                    if (createdTimeEnd != null)
                    {
                        createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                        query = query.Where(x => x.verify_time < createdTimeEnd);
                    }
                    count = query.Count();
                    list = query.OrderByDescending(x => x.verify_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }


                if (userId == 0)
                {
                    var userIdList = list.Select(x => x.app_user_id).ToList();
                    var userDic = db.app_user.Where(x => userIdList.Contains(x.id) && x.statu == EntityStatu.normal).ToDictionary(x => x.id);
                    var projectIdList = list.Select(x => x.project_id).ToList();
                    var prpjectDic = db.project.Where(x => projectIdList.Contains(x.id) && x.statu == EntityStatu.normal).ToDictionary(x => x.id);
                    list.ForEach(x =>
                    {
                        if (x.app_user_id != 0 && userDic.ContainsKey(x.app_user_id))
                        {
                            x.app_user_name = userDic[x.app_user_id].username;
                        }
                        if (x.project_id != 0 && prpjectDic.ContainsKey(x.project_id))
                        {
                            var project = prpjectDic[x.project_id];
                            x.price = project.price;
                            x.project_name = project.name;
                            x.price_discount = project.price_discount;
                            x.divide_type = project.divide_type;
                            x.divide_fixed_amount = project.divide_fixed_amount;
                            x.divide_ratio_value = project.divide_ratio_value;
                        }
                    });
                }
                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WebResult<bool> Audit(int id, AuditState state)
        {
            if (id <= 0)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            var model = Find(id);
            if (model == null)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            if (model.invite_state == AuditState.success)
            {
                return Result(false, ErrorCode.had_audit);
            }
            if (state == AuditState.success)
            {
                var user = appUserService.Find(model.app_user_id);
                var fromUser = appUserService.Find(model.from_user_id);

                inviteService = new  InviteService();
                inviteService.Add(new app_user_bill()
                {
                    tran_type = TranType.invate,
                    before_funds = user.invite_funds,
                    amount = model.amount,
                    after_funds = user.invite_funds + model.amount,
                    app_user_id = model.app_user_id,
                    create_time = DateTime.Now,
                    order_info = $"{user.username}获得推广提成{model.amount}",

                });
                //新增上下级贡献金钱
                user.invite_profit += model.amount;
                user.invite_funds += model.amount;
                fromUser.invite_offer += model.amount;
                appUserService.Update(user);
                appUserService.Update(fromUser);
            }
            
            int result = Update(model);
            if (result > 0)
            {
                return Result(true);
            }
            else
            {
                return Result(false, ErrorCode.sys_fail);
            }
        }
    }
}
