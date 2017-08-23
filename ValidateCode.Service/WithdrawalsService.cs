
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
    public class WithdrawalsService : BaseService<withdrawals>, IWithdrawalsService
    {
        private AppUserService userService;
        public PageList<withdrawals> GetPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd, int userId = 0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<withdrawals>();
                var count = 0;
                if (userId != 0)
                {

                    var query = db.withdrawals.Where(x => x.statu == EntityStatu.normal && x.app_user_id == userId);
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
                    var query = db.withdrawals.Where(x => x.statu == EntityStatu.normal);
                    if (name.IsNotNullOrEmpty())
                    {
                        var userIdList = db.app_user.Where(x => x.statu == EntityStatu.normal && x.username.Contains(name)).Select(x => x.id).ToList();
                        if (userIdList != null && userIdList.Count > 0)
                        {
                            query = query.Where(x => userIdList.Contains(x.app_user_id));
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


                if (userId == 0)
                {
                    var userIdList = list.Select(x => x.app_user_id).ToList();
                    var userDic = db.app_user.Where(x => userIdList.Contains(x.id) && x.statu == EntityStatu.normal).ToDictionary(x => x.id);
                    list.ForEach(x =>
                    {
                        if (x.app_user_id != 0 && userDic.ContainsKey(x.app_user_id))
                        {
                            x.app_user_name = userDic[x.app_user_id].username;
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
        public WebResult<bool> Audit(int id, AuditState state, PayType? type, string orderId)
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
                return Result(false, ErrorCode.had_audit);
            }
            if (state == AuditState.success)
            {
                model.type = type.Value;
                model.third_order_id = orderId;
                userService = new AppUserService();

                var user = userService.Find(model.app_user_id);
                if (user == null)
                    return Result(false, ErrorCode.sys_param_format_error);
                if (user.invite_funds < model.amount)
                    return Result(false, ErrorCode.invite_amount_error);

                var bill = new app_user_bill();
                bill.tran_type = TranType.withdrawls;
                bill.tran_type_source = model.id;
                bill.order_info = $"用户{user.username},申请提现金额{model.amount}";
                bill.create_time = DateTime.Now;
                bill.app_user_id = model.app_user_id;
                bill.amount = model.amount;
                //提现表修改
                model.before_funds = user.invite_funds;
                model.after_funds = user.invite_funds - model.amount;
                user.invite_funds -= model.amount;
                Update(model);
                //用户修改
                userService.Update(user);
                using (DbRepository db = new DbRepository())
                {
                    db.app_user_bill.Add(bill);
                    db.SaveChanges();
                }

                return Result(true);
            }
            else
            {
                model.audit_state = state;
                model.audit_time = DateTime.Now;
                Update(model);
                return Result(true);
            }
        }
    }
}
