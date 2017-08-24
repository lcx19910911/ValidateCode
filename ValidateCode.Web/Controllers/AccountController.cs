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
using ValidateCode.Core.Code;

namespace ValidateCode.Web.Controllers
{
    public class AccountController : BaseController
    {

        public IAppUserService IUserService;

        public IAdminUserService IAdminUserService;
        public AccountController(IAppUserService _IUserService, IAdminUserService _IAdminUserService)
        {
            this.IUserService = _IUserService;
            this.IAdminUserService = _IAdminUserService;
        }

        
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        // GET: Login
        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult Update(int id = 0)
        {
            var model = IUserService.Find(id);
            if (model == null)
                return View(new app_user());
            else
                return View(model);
        }

        public ActionResult Register(int id=0)
        {
            var model = IUserService.Find(id);
            if (model == null)
                return View(new app_user());
            else
                return View(model);
        }

        [HttpPost]
        public ActionResult Register(app_user model,string code, string invite_user_code)
        {
            ModelState.Remove("id");
            ModelState.Remove("reg_time");
            ModelState.Remove("pasword");
            if (ModelState.IsValid)
            {
                var result = IUserService.Manager(model, code,false, invite_user_code);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }


        [HttpPost]
        public ActionResult Save(app_user model, string code,string invite_user_code)
        {
            
            ModelState.Remove("reg_time");
            ModelState.Remove("pasword");
            ModelState.Remove("username");
            if (ModelState.IsValid)
            {
                var result = IUserService.Manager(model, code, false, invite_user_code);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }


        [HttpPost]
        public ActionResult CreateAdmin(admin_user model)
        {

            ModelState.Remove("id");
            ModelState.Remove("created_time");
            if (ModelState.IsValid)
            {
                var result = IAdminUserService.Manager(model);
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
                if(result.Result==null)
                    return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "账号密码错误" });
            }
            return JResult(result);

        }


        /// <summary>
        /// 登录提交
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param> 
        /// <returns></returns>
        public JsonResult AdminSubmit(string account, string password, string code)
        {
            var result = IAdminUserService.Login(account, password, code);
            if (result.Success)
            {
                if (result.Result == null)
                    return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "账号密码错误" });
                LoginHelper.CreateUser(new LoginUser(result.Result.id, result.Result.account, true), Params.AdminCookieName);
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


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminQuit()
        {
            this.LoginUser = null;
            return View("AdminLogin");
        }


        public ActionResult ChangePassword(string pasword, string new_password, string confirm_password,int id=0)
        {
            return JResult(IUserService.ChangePassword(pasword, new_password, confirm_password, id==0?this.LoginUser.ID:id));
        }
        public ActionResult ChangeAdminPassword(string pasword, string new_password, string confirm_password)
        {
            return JResult(IAdminUserService.ChangePassword(pasword, new_password, confirm_password, this.LoginAdmin.ID));
        }
    }
}