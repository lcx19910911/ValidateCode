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
    public class WithdrawalsController : BaseController
    {
        public IWithdrawalsService IWithdrawalsService;
        public IAppUserService IAppUserService;

        public WithdrawalsController(IWithdrawalsService _IWithdrawalsService, IAppUserService _IAppUserService)
        {
            this.IWithdrawalsService = _IWithdrawalsService;
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
        public ActionResult GetPageList(int pageIndex, int pageSize, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            return JResult(IWithdrawalsService.GetPageList(pageIndex, pageSize,null, createdTimeStart, createdTimeEnd, this.LoginUser.ID));
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(decimal amount)
        {
            var model = new app_user_bill();
            model.tran_type = TranType.withdrawls;
            model.create_time = DateTime.Now;
            model.audit_state = AuditState.wait;
            model.app_user_id = this.LoginUser.ID;
            model.type = PayType.alipay;
            model.amount = amount;
            var user = IAppUserService.Find(this.LoginUser.ID);
            if (user == null)
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "操作失败" });
            if (user.invite_funds < model.amount)
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "提现金额超出用户提成总额" });
            model.before_funds = user.invite_funds;
            model.after_funds = user.invite_funds - model.amount;
            model.order_info = $"用户{this.LoginUser.Account},申请提现金额{model.amount}";
            var result = IWithdrawalsService.Add(model);
            if (result > 0)
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = true });
            else
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "操作失败" });
        }
    }
}