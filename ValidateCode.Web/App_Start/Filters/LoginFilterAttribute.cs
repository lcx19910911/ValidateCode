﻿using ValidateCode.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ValidateCode.Web.Filters
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginFilterAttribute : ActionFilterAttribute
    {
     

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            var areaName = filterContext.RouteData.DataTokens["area"];
            var actionName = filterContext.RouteData.Values["Action"].ToString();
            var controllerName = filterContext.RouteData.Values["Controller"].ToString();
            var actionMethodList = filterContext.Controller.GetType().GetMethods();
            string requestUrl = filterContext.HttpContext.Request.Url.ToString();
            string token = filterContext.HttpContext.Request["token"];
            string info = filterContext.HttpContext.Request["info"];


            if (areaName != null)
            {
                if (controller.LoginAdmin == null)
                {
                    RedirectResult redirectResult = new RedirectResult("/account/adminlogin?redirecturl=" + requestUrl);
                    filterContext.Result = redirectResult;
                }
            }
            else
            {

                if (controller.LoginUser == null)
                {
                    if (!controllerName.Equals("account", StringComparison.OrdinalIgnoreCase))
                    {
                        var actionMethod = actionMethodList.Where(x => x.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (actionMethod != null)
                        {
                            if (actionMethod.ReturnType.Name == "ViewResult" || actionMethod.ReturnType.Name == "ActionResult")
                            {
                                RedirectResult redirectResult = new RedirectResult("/account/login?redirecturl=" + requestUrl);
                                filterContext.Result = redirectResult;
                            }
                            else if (actionMethod.ReturnType.Name == "JsonResult")
                            {
                                JsonResult jsonResult = new JsonResult();
                                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                                filterContext.RequestContext.HttpContext.Response.StatusCode = 9999;
                                filterContext.Result = jsonResult;
                            }
                        }
                    }
                }
            }
        }
    }
}