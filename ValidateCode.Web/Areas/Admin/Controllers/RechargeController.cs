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

namespace ValidateCode.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [LoginFilter]
    public class RechargeController : BaseAdminController
    {
        public IRechargeService IRechargeService;
        public IAppUserService IAppUserService;


        public RechargeController(IRechargeService _IRechargeService, IAppUserService _IAppUserService)
        {
            this.IRechargeService = _IRechargeService;
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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name,DateTime? createdTimeStart,DateTime? createdTimeEnd)
        {
            return JResult(IRechargeService.GetPageList(pageIndex, pageSize, name, createdTimeStart, createdTimeEnd));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(int id)
        {
            return JResult(IRechargeService.Find(id));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(app_user_bill model)
        {
            ModelState.Remove("id");
            ModelState.Remove("create_time");
            ModelState.Remove("audit_state");
            ModelState.Remove("order_info");
            if (ModelState.IsValid)
            {
                model.tran_type = TranType.recharge;
                model.audit_state = AuditState.success;
                model.audit_time = DateTime.Now;
                model.create_time = DateTime.Now;
                var user = IAppUserService.Find(model.app_user_id);
                if (user != null)
                    return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "操作失败" });
                model.before_funds = user.funds;
                model.after_funds = user.funds + model.amount;
                model.order_info = $"给用户{user.username},充值金额{model.amount}";
                var result = IRechargeService.Add(model);
                if (result > 0)
                    return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = true });
                else
                    return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "操作失败" });
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }
    }
}