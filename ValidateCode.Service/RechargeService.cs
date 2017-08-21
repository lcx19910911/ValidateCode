
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

namespace ValidateCode.Service
{
    /// <summary>
    /// 用户充值记录
    /// </summary>
    public class RechargeService : BaseService<recharge>, IRechargeService
    {
        public PageList<recharge> GetPageList(int pageIndex, int pageSize,  DateTime? createdTimeStart, DateTime? createdTimeEnd, int userId=0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<recharge>();
                var returnList = new List<recharge>();
                var count = 0;
                if (userId!=0)
                {
                    var user = db.app_user.Find(userId);
                    if (user != null&&user.UserRecharges!=null&&user.UserRecharges.Count>0)
                    {
                        var query = user.UserRecharges.AsQueryable().Include("User");
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
                    
                }
                else
                {
                    var query = db.recharge.AsQueryable().Include("CreaterUser");
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
                    //returnList.Add(new Recharge()
                    //{
                    //    UserName=x.User.NickName,
                    //    CreaterUserName=x.CreaterUser.NickName,
                    //    CreatedTime=x.CreatedTime,
                    //    Count=x.Count,
                    //    ID=x.ID,
                    //    VoucherImg=x.VoucherImg,
                    //    VoucherNo=x.VoucherNo
                    //});
                });
                return CreatePageList(returnList, pageIndex, pageSize, count);
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
                return Result(false, Core.Code.ErrorCode.sys_param_format_error);
            }
            var model = Find(id);
            if (model == null)
            {
                return Result(false, Core.Code.ErrorCode.sys_param_format_error);
            }
            if (model.order_state == PayStatu.success)
            {
                return Result(false, Core.Code.ErrorCode.had_audit);
            }


        }
    }
}
