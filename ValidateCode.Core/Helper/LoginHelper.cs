﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ValidateCode.Core.Extensions;
using ValidateCode.Core.Helper;
using ValidateCode.Core.Model;
using ValidateCode.Model;

namespace ValidateCode.Core
{
    public static class LoginHelper
    {
       

        public static void CreateUser(LoginUser user,string cookieName)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = CryptoHelper.AES_Encrypt(user.ToJson(), Params.SecretKey);
            cookie.Expires = DateTime.Now.AddHours(1);
            // 写登录Cookie
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);

            //userToken
            if (user!=null&&user.Token.IsNotNullOrEmpty())
            {
                HttpCookie userTokencookie = new HttpCookie(Params.UserTokenCookieName);
                userTokencookie.Value = user.Token;
                userTokencookie.Expires = DateTime.Now.AddHours(1);
                HttpContext.Current.Response.Cookies.Remove(userTokencookie.Name);
                HttpContext.Current.Response.Cookies.Add(userTokencookie);
            }
        }



        public static void ClearUser(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddHours(-1);
                HttpContext.Current.Response.Cookies.Remove(cookie.Name);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }


        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static LoginUser GetCurrentUser()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Params.UserCookieName];
            if (cookie == null)
                return null;
            var user = (CryptoHelper.AES_Decrypt(cookie.Value, Params.SecretKey)).DeserializeJson<LoginUser>();
            return user;
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static LoginUser GetCurrentAdmin()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Params.AdminCookieName];
            if (cookie == null)
                return null;
            var user = (CryptoHelper.AES_Decrypt(cookie.Value, Params.SecretKey)).DeserializeJson<LoginUser>();
            return user;
        }


        /// <summary>
        /// 是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool UserIsLogin()
        {
            return GetCurrentUser() != null;
        }
    }
}
