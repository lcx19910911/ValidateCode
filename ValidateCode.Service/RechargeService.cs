
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ValidateCode.Core.Model;
using ValidateCode.DB;
using ValidateCode.IService;
using ValidateCode.Model;
using ValidateCode.Core.Extensions;
using ValidateCode.Core.Code;

namespace ValidateCode.Service
{
    /// <summary>
    /// 用户充值记录
    /// </summary>
    public class RechargeService : BaseService<app_user_bill>, IRechargeService
    {
        public PageList<app_user_bill> GetPageList(int pageIndex, int pageSize, string name,  DateTime? createdTimeStart, DateTime? createdTimeEnd, int userId=0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<app_user_bill>();
                var count = 0;
                if (userId!=0)
                {
                    var query = db.app_user_bill.Where(x => x.tran_type == TranType.recharge && x.statu == EntityStatu.normal && x.app_user_id == userId);
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
                    var query = db.app_user_bill.Where(x => x.tran_type == TranType.recharge && x.statu == EntityStatu.normal);
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
        public WebResult<bool> HadPay(int id,string orderId,PayType type)
        {
            if (id <= 0||orderId.IsNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            var model = Find(id);
            if (model == null)
            {
                return Result(false, Core.Code.ErrorCode.sys_param_format_error);
            }
            if (model.audit_state == AuditState.success)
            {
                return Result(false, Core.Code.ErrorCode.had_audit);
            }

            model.third_order_id = orderId;
            model.type = type;
            model.audit_state = AuditState.success;

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
