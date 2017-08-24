
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ValidateCode.Core;
using ValidateCode.Core.Code;
using ValidateCode.Core.Extensions;
using ValidateCode.Core.Helper;
using ValidateCode.Core.Model;
using ValidateCode.Core.Web;
using ValidateCode.DB;
using ValidateCode.IService;
using ValidateCode.Model;

namespace ValidateCode.Service
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class AppUserService : BaseService<app_user>, IAppUserService
    {
        public AppUserService()
        {
            base.ContextCurrent = HttpContext.Current;
        }
        private UserTokenService userTokenService;

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public WebResult<app_user> Login(string name, string password,string code)
        {
            using (DbRepository entities = new DbRepository())
            {
                var realCode = CacheHelper.Get<string>("validate_code" + Client.IP);
                if (code.IsNullOrEmpty() || !code.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Result(new app_user() { }, ErrorCode.verification_time_out);
                }

                var user = Find(x => x.username == name && x.pasword == password && x.statu != EntityStatu.delete);
                if (user != null)
                {
                    user.login_ip = Client.IP;
                    user.login_time = DateTime.Now;

                    var userToken = new user_token()
                    {
                        user_id = user.id,
                        token = Guid.NewGuid().ToString("N"),
                        create_time = DateTime.Now,
                        expire_time = DateTime.Now.AddHours(1),
                        access_time = DateTime.Now.AddHours(1),
                        device_ip = this.Client.IP
                    };
                    userTokenService = new UserTokenService();
                    userTokenService.Add(userToken);
                    Update(user);
                    Client.LoginUser = new LoginUser()
                    {
                        ID = user.id,
                        Account = user.username,
                        IsAdmin = false,
                        Token = userToken.token
                    };
                };
                
                return Result(user);
            }
        }

        /// <summary>
        /// 编辑管理用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Manager(app_user model, string code, bool isAdmin,string invite_user_code)
        {
            if (!isAdmin)
            {
                var realCode = CacheHelper.Get<string>("validate_code" + Client.IP);
                if (code.IsNullOrEmpty() || !code.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Result(false, ErrorCode.verification_time_out);
                }
            }
            var user = Find(model.id);
            if (user == null)
            {
                //生成用户邀请码
                var invateCode = new Random().Next(100000, 999999).ToString();
                var isExit = true;
                while (isExit)
                {
                    if (IsExits(x => x.invite_code == invateCode))
                    {
                        invateCode = new Random().Next(100000, 999999).ToString();
                    }
                    else
                    {
                        isExit = false;
                        model.invite_code = invateCode;
                    }
                }
                //检查邀请码
                if (!IsExits(x => x.invite_code == invite_user_code))
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
                var inviteUser = Find(x => x.invite_code == invite_user_code && x.statu == EntityStatu.normal);
                if (inviteUser == null)
                {
                    return Result(false, ErrorCode.invite_code_error);
                }
                model.invite_user_id = inviteUser.id;
                model.pasword = model.new_password;
                model.reg_time = DateTime.Now;
                int id=Add(model);
                if (id > 0)
                {
                    LoginHelper.CreateUser(new LoginUser(id,model.username,false), Params.UserCookieName);
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }
            else
            {

                if (IsExits(x => x.username == model.username && x.id != model.id))
                    return Result(false, ErrorCode.user_account_already_exist);

                user.alipay_acc = model.alipay_acc;
                user.alipay_name = model.alipay_name;
                user.qq = model.qq;
                user.phone = model.phone;
                user.email = model.email;
                if (model.vip_level.HasValue)
                {
                    user.vip_level = model.vip_level.Value;
                }
                int result=Update(user);
                if (result > 0)
                {
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }

        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<app_user> GetPageList(int pageIndex, int pageSize, string account,string phone,string code)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.app_user.Where(x => x.statu != EntityStatu.delete);
                if (account.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.username.Contains(account));
                }
                if (phone.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.phone.Contains(phone));
                }
                if (code.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.invite_code.Contains(code));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.reg_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var userIdList = list.Select(x => x.invite_user_id).ToList();
                var userDic = db.app_user.Where(x => userIdList.Contains(x.id) && x.statu == EntityStatu.normal).ToDictionary(x => x.id);
                list.ForEach(x =>
                {
                    if (x .invite_user_id.HasValue&&userDic.ContainsKey(x.invite_user_id.Value))
                    {
                        x.invite_user_name = userDic[x.invite_user_id.Value].username;
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }


        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns> 
        public WebResult<bool> ChangePassword(string oldPassword, string newPassword, string cfmPassword, int id)
        {
            if (newPassword.IsNullOrEmpty() || cfmPassword.IsNullOrEmpty() || id==0)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            if (!cfmPassword.Equals(newPassword))
            {
                return Result(false, ErrorCode.user_password_notequal);

            }
            var user = Find(id);
            if (user == null)
                return Result(false, ErrorCode.user_not_exit);
            user.pasword = newPassword;
            Update(user);
            return Result(true);
        }


    }
}
