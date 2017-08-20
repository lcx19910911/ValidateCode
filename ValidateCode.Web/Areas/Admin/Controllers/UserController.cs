using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidateCode.IService;
using ValidateCode.Model;
using ValidateCode.Web.Filters;

namespace ValidateCode.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [LoginFilter]
    public class UserController : BaseAdminController
    {
        public IAppUserService IAppUserService;

        public UserController(IAppUserService _IUserService)
        {
            this.IAppUserService = _IUserService;
        }
        // GET: 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string name,string phone,string code)
        {
            return JResult(IAppUserService.GetPageList(pageIndex, pageSize, name, phone,code));
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IAppUserService.Delete(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(int id)
        {
            return JResult(IAppUserService.Find(id));
        }

        [HttpPost]
        public ActionResult Update(app_user model)
        {
            var result = IAppUserService.Manager(model,"",true);
            return JResult(result);
        }

        public ActionResult ChangePassword(string oldPassword, string newPassword, string cfmPassword, int id)
        {
            return JResult(IAppUserService.ChangePassword(oldPassword, newPassword, cfmPassword, id));
        }
    }
}