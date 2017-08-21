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
    public class RechargeController : BaseController
    {
        public IRechargeService IRechargeService;

        public RechargeController(IRechargeService _IRechargeService)
        {
            this.IRechargeService = _IRechargeService;
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
        public ActionResult GetPageList(int pageIndex, int pageSize,DateTime? createdTimeStart,DateTime? createdTimeEnd)
        {
            return JResult(IRechargeService.GetPageList(pageIndex, pageSize,null, createdTimeStart, createdTimeEnd, this.LoginUser.ID));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(int id)
        {
            return JResult(IRechargeService.Find(id));
        }

    }
}