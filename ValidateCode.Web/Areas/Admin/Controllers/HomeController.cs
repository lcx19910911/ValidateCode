using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidateCode.Web.Filters;

namespace ValidateCode.Web.Areas.Admin.Controllers
{
    [LoginFilter]
    public class HomeController : BaseAdminController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}