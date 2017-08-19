using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;
using ValidateCode.Core.Helper;

namespace ValidateCode.Core
{
    public class Params
    {
        /// <summary>
        /// 时间戳有效时间c
        /// </summary>
        public const int TimspanExpiredMinutes = 60;
        /// <summary>
        /// token失效时间
        /// </summary>
        public const int ExpiredDays = 7;

        public static readonly string SecretKey =CustomHelper.GetValue("SecretKey");
        public static readonly string DomianName = CustomHelper.GetValue("Company_SiteUrl");
        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserCookieName = "website_user";
    }
}
