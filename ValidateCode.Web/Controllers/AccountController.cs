using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using NLog;
using ValidateCode.IService;
using ValidateCode.Core;
using ValidateCode.Core.Extensions;
using ValidateCode.Core.Web;
using ValidateCode.Core.Model;
using ValidateCode.Model;

namespace ValidateCode.Web.Controllers
{
    public class AccountController : BaseController
    {

        public IUserService IUserService;

        public AccountController(IUserService _IUserService)
        {
            this.IUserService = _IUserService;
        }

        
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {

            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("IsDelete");
            ModelState.Remove("Password");
            ModelState.Remove("Type");
            if (ModelState.IsValid)
            {
                var result = IUserService.Manager(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }

        #region 验证码
            /// <summary>
            /// 验证码
            /// </summary>
        public void ValidateCode()
        {
            ValidateCodeGenertor v = ValidateCodeGenertor.Default;
            CacheHelper.Remove("code" + this.IP);
            var code = v.CreateValidateCode();
            CacheHelper.Set<string>("code" + this.IP, code, CacheTimeOption.SixMinutes);
            var tuple= new Tuple<ValidateCodeGenertor, string>(v, code);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.ClearContent();
            HttpContext.Response.ContentType = "image/Jpeg";
            HttpContext.Response.BinaryWrite(tuple.Item1.CreateImageStream(tuple.Item2));    //  输出图片
        }
        #endregion

        /// <summary>
        /// 登录提交
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param> 
        /// <returns></returns>
        public JsonResult Submit(string account, string password,string code)
        {
            var result = IUserService.Login(account, password,code);
            if (result.Success)
            {
                LoginHelper.CreateUser(result.Result);
            }
            return JResult(result);

        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            this.LoginUser = null;
            return View("Login");
        }
    }
}