
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

        private AppUserService userService;


        public PageList<invite_bill> GetPageList(int pageIndex, int pageSize,string name,DateTime? createdTimeStart, DateTime? createdTimeEnd,int userId=0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<invite_bill>();
                var count = 0;
                if (userId != 0)
                {

                    var query = db.invite_bill.Where(x => x.statu == EntityStatu.normal && x.app_user_id == userId&&x.invite_state==ProjectState.enable);
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
    }
}
