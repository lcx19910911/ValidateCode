using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidateCode.IService;
using ValidateCode.Web.Filters;

namespace ValidateCode.Web.Controllers
{
    [LoginFilter]
    public class HomeController : BaseController
    {
        public IAppUserService IAppUserService;

        public HomeController(IAppUserService _IAppUserService)
        {
            this.IAppUserService = _IAppUserService;
        }
        // GET: Home
        public ActionResult Index()
        {
            var user = IAppUserService.Find(this.LoginUser.ID);
            return View(user);
        }
    }
}