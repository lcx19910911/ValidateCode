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
        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserTokenCookieName = "token";
        /// 登陆cookie
        /// </summary>
        public static readonly string AdminCookieName = "website_admin";

        /// <summary>
        /// 已收短信表(statu)  只对用户显示该值的短信
        /// </summary>
        public const int ShowSendSmsStatus = 2000;

        /// <summary>
        /// 已发短信表sms_send  的statu  只对用户显示该值的短信
        /// </summary>
        public const int ShowReviceSmsStatus = 1900;
    }
}
