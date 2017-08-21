using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidateCode.IService;
using ValidateCode.Model;
using ValidateCode.Web.Filters;

namespace ValidateCode.Web.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [LoginFilter]
    public class ProjectController : BaseController
    {
        public IProjectService IProjectService;

        public ProjectController(IProjectService _IProjectService)
        {
            this.IProjectService = _IProjectService;
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name)
        {
            return JResult(IProjectService.GetPageList(pageIndex, pageSize, name,false));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(int id)
        {
            return JResult(IProjectService.Find(id));
        }

    }
}