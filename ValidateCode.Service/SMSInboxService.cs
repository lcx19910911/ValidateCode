
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
    public class SMSInboxService : BaseService<sms_inbox>, ISMSInboxService
    {

        private AppUserService userService;


        public PageList<sms_inbox> GetPageList(int pageIndex, int pageSize,string name,string userName,string phone,DateTime? createdTimeStart, DateTime? createdTimeEnd,int userId=0)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<sms_inbox>();
                var count = 0;
                if (userId!=0)
                {
                    
                        var query =db.sms_inbox.Where(x=>x.statu==EntityStatu.smsInbox && x.app_user_id==userId);
                    if (name.IsNotNullOrEmpty())
                    {
                        query = query.Where(x => x.project_name.Contains(name));
                    }
                    if (phone.IsNotNullOrEmpty())
                    {
                        query = query.Where(x => x.phone_num.Contains(phone));
                    }
                    if (createdTimeStart != null)
                        {
                            query = query.Where(x => x.record_time >= createdTimeStart);
                        }
                        if (createdTimeEnd != null)
                        {
                            createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                            query = query.Where(x => x.record_time < createdTimeEnd);
                        }
                        count = query.Count();
                        list = query.OrderByDescending(x => x.record_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();             
                }
                else
                {
                    var query = db.sms_inbox.Where(x=> x.statu == EntityStatu.smsInbox);
                    if (name.IsNotNullOrEmpty())
                    {
                        query = query.Where(x => x.project_name.Contains(name));
                    }
                    if (phone.IsNotNullOrEmpty())
                    {
                        query = query.Where(x => x.phone_num.Contains(phone));
                    }
                    if (userName.IsNotNullOrEmpty())
                    {
                        var userIdList = db.app_user.Where(x => x.statu == EntityStatu.normal && x.username.Contains(userName)).Select(x => x.id).ToList();
                        if (userIdList != null && userIdList.Count > 0)
                        {
                            query = query.Where(x => userIdList.Contains(x.app_user_id.Value));
                        }
                    }
                    if (createdTimeStart != null)
                    {
                        query = query.Where(x => x.record_time >= createdTimeStart);
                    }
                    if (createdTimeEnd != null)
                    {
                        createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                        query = query.Where(x => x.record_time < createdTimeEnd);
                    }
                    count = query.Count();
                    list = query.OrderByDescending(x => x.record_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }


                if (userId == 0)
                {
                    var userIdList = list.Select(x => x.app_user_id).ToList();
                    var userDic = db.app_user.Where(x => userIdList.Contains(x.id) && x.statu == EntityStatu.normal).ToDictionary(x => x.id);
                    list.ForEach(x =>
                    {
                        if (x.app_user_id.HasValue && userDic.ContainsKey(x.app_user_id.Value))
                        {
                            x.app_user_name = userDic[x.app_user_id.Value].username;
                        }
                    });
                }
                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }
        
    }
}
