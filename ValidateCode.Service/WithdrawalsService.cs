
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
    public class WithdrawalsService : BaseService<app_user_bill>, IWithdrawalsService
    {

        private AppUserService userService;


        public PageList<app_user_bill> GetPageList(int pageIndex, int pageSize,string name,DateTime? createdTimeStart, DateTime? createdTimeEnd,int userId=0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<app_user_bill>();
                var returnList = new List<app_user_bill>();
                var count = 0;
                if (userId!=0)
                {
                    
                        var query =db.app_user_bill.Where(x=>x.tran_type==TranType.withdrawls&&x.statu==EntityStatu.normal&&x.app_user_id==userId);
                        if (createdTimeStart != null)
                        {
                            query = query.Where(x => x.create_time >= createdTimeStart);
                        }
                        if (createdTimeEnd != null)
                        {
                            createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                            query = query.Where(x => x.create_time < createdTimeEnd);
                        }
                        count = query.Count();
                        list = query.OrderByDescending(x => x.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();             
                }
                else
                {
                    var query = db.app_user_bill.Where(x=> x.tran_type == TranType.withdrawls && x.statu == EntityStatu.normal);
                    if (name.IsNotNullOrEmpty())
                    {
                        var userIdList = db.app_user.Where(x => x.statu == EntityStatu.normal && x.username.Contains(name)).Select(x => x.id).ToList();
                        if (userIdList != null && userIdList.Count > 0)
                        {
                            query = query.Where(x => userIdList.Contains(x.id));
                        }
                    }
                    if (createdTimeStart != null)
                    {
                        query = query.Where(x => x.create_time >= createdTimeStart);
                    }
                    if (createdTimeEnd != null)
                    {
                        createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                        query = query.Where(x => x.create_time < createdTimeEnd);
                    }
                    count = query.Count();
                    list = query.OrderByDescending(x => x.create_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }


                list.ForEach(x =>
                {
                    //returnList.Add(new Withdrawals()
                    //{
                    //    UserName = x.User.NickName,
                    //    AuditUserName = x.AuditUser?.NickName,
                    //    CreatedTime = x.CreatedTime,
                    //    Count = x.Count,
                    //    ID = x.ID,
                    //    VoucherImg = x.VoucherImg,
                    //    VoucherNo = x.VoucherNo,
                    //    StateStr=x.State.GetDescription(),
                    //    State=x.State
                    //});
                });
                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WebResult<bool> Audit(int id, AuditState state,PayType? type,string orderId)
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
            if (model.audit_state == AuditState.success)
            {
                return Result(false,ErrorCode.had_audit);
            }
            model.audit_time = DateTime.Now;
            if (state == AuditState.success)
            {
                model.type = type.Value;
                model.third_order_id = orderId;
                userService = new AppUserService();
                var user = userService.Find(model.app_user_id);
                if (user == null)
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
                user.invite_funds -= model.amount;
            }
            
            model.audit_state = state;
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
