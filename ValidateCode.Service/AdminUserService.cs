
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
    public class AdminUserService : BaseService<admin_user>, IAdminUserService
    {
        public AdminUserService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public WebResult<admin_user> Login(string name, string password,string code)
        {
            using (DbRepository entities = new DbRepository())
            {
                var realCode = CacheHelper.Get<string>("validate_code" + Client.IP);
                if (code.IsNullOrEmpty() || !code.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Result(new admin_user() { }, ErrorCode.verification_time_out);
                }
                string md5Password = Core.Util.CryptoHelper.MD5_Encrypt(password);

                return Result(Find(x => x.account == name && x.password == md5Password && x.statu != EntityStatu.delete));
            }
        }

        /// <summary>
        /// 编辑管理用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Manager(admin_user model)
        {
            var user = Find(model.id);
            if (user == null)
            {
                int id=Add(model);
                if (id > 0)
                {
                    LoginHelper.CreateUser(new LoginUser(id,model.account,false));
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }
            else
            {

                if (IsExits(x => x.account == model.account && x.id != model.id))
                    return Result(false, ErrorCode.user_account_already_exist);
              
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
        public PageList<admin_user> GetPageList(int pageIndex, int pageSize, string account)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.admin_user.Where(x => x.statu != EntityStatu.delete);
                if (account.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.account.Contains(account));
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.created_time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                list.ForEach(x =>
                {
                    if (x != null)
                    {
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
            if (oldPassword == "")
            {
                oldPassword = CryptoHelper.MD5_Encrypt(oldPassword);
                if (!user.password.Equals(oldPassword))
                    return Result(false, ErrorCode.user_password_nottrue);
            }
            newPassword = CryptoHelper.MD5_Encrypt(newPassword);
            user.password = newPassword;
            Update(user);
            return Result(true);
        }


    }
}
