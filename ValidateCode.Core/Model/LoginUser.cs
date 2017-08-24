
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateCode.Core.Model
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(int id,string account,bool isAdmin)
        {
            this.ID = id;
            this.Account = account;
            this.IsAdmin = isAdmin;
        }


        public LoginUser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }


        public string Token { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
