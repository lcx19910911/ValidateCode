using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ValidateCode.Core.Util;
using ValidateCode.DB;

namespace ValidateCode.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogHelper.WriteCustom(string.Format("Application_Start At {0} \r\n", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")), @"Application\", false);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Database.SetInitializer<DbRepository>(null);
            ///预加载
            using (var dbcontext = new DbRepository())
            {
                //dbcontext.Database.CreateIfNotExists();
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
                if (!dbcontext.User.Where(x => x.Type==Model.UserType.Admin).Any())
                {
                    dbcontext.User.Add(new Model.User()
                    {
                        CreatedTime = DateTime.Now,
                        Account = "admin",
                        Type = Model.UserType.Admin,
                        Password = CryptoHelper.MD5_Encrypt("123456"),
                        AliPayName= "admin",
                        AliPayAccount="admin"
                    });
                    dbcontext.SaveChanges();
                }
            }
        }



        protected void Application_End(object sender, EventArgs e)
        {
            var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
            if (runtime != null)
            {
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                ApplicationShutdownReason shutDownReason = (ApplicationShutdownReason)runtime.GetType().InvokeMember("_shutdownReason", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                LogHelper.WriteCustom(string.Format("Application_End:shutDownMessage:\r\n{0}\r\nshutDownStack:\r\n{1}\r\nshutDownReason:\r\n{2}\r\n", shutDownMessage, shutDownStack, shutDownReason.ToString()), @"Application\", false);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception is ThreadAbortException)
            {
                Thread.ResetAbort();
                HttpContext.Current.ClearError();
                return;
            }
            var httpException = exception as HttpException;

            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                LogHelper.WriteCustom(httpException.ToString(), "404Error\\");
                Server.ClearError();
                Response.Clear();
                Response.Redirect("/base/_404");
            }
            else
            {
                LogHelper.WriteException("Application Error.", Server.GetLastError());
                Server.ClearError();
                Response.Clear();
                Response.Redirect("/base/_500");
            }

        }
    }
}
