using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidateCode.Core.Code;
using ValidateCode.Core.Model;
using ValidateCode.IService;
using ValidateCode.Model;
using ValidateCode.Web.Filters;

namespace ValidateCode.Web.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [LoginFilter]
    public class SMSSendController : BaseController
    {
        public ISMSSendService ISMSSendService;
        public IAppUserService IAppUserService;

        public SMSSendController(ISMSSendService _ISMSSendService, IAppUserService _IAppUserService)
        {
            this.ISMSSendService = _ISMSSendService;
            this.IAppUserService = _IAppUserService;
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
        public ActionResult GetPageList(int pageIndex, int pageSize,string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            return JResult(ISMSSendService.GetPageList(pageIndex, pageSize, name, createdTimeStart, createdTimeEnd, this.LoginUser.ID));
        }
    }
}