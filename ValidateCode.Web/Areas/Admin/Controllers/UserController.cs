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
    public class UserController : BaseAdminController
    {
        public IAppUserService IAppUserService;
        public IRechargeService IRechargeService;


        public UserController(IAppUserService _IUserService, IRechargeService _IRechargeService)
        {
            this.IAppUserService = _IUserService;
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
        public ActionResult Recharge(int app_user_id, decimal amount, PayType type,string third_order_id)
        {
            var user = IAppUserService.Find(app_user_id);
            if (user != null)
            {
                var model = new app_user_bill()
                {
                    app_user_id = app_user_id,
                    amount = amount,
                    type = type,
                    tran_type = TranType.recharge,
                    audit_state = AuditState.success,
                    audit_time = DateTime.Now,
                    create_time = DateTime.Now,
                    order_info = $"{user.username}充值{amount}",
                    before_funds = user.funds,
                    after_funds = user.funds + amount,
                    third_order_id = third_order_id,
                };
                user.funds += amount;
                IAppUserService.Update(user);
                var result = IRechargeService.Add(model);
                return JResult(result);
            }
            else
            {
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_param_format_error, Result = false, Append = "参数错误" });
            }
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