using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidateCode.Core.Code;
using ValidateCode.Core.Model;
using ValidateCode.DB;
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

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge(int app_user_id, decimal amount, PayType type,string third_order_id)
        {

            var model = new recharge();
            model.app_user_id = app_user_id;
            model.amount = amount;
            model.type = type;
            model.third_order_id = third_order_id;
            model.create_time = DateTime.Now;
            var user = IAppUserService.Find(app_user_id);
            if (user == null)
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "操作失败" });
            model.before_funds = user.funds;
            model.after_funds = user.funds + model.amount;

            var result = IRechargeService.Add(model);
            if (result > 0)
            {

                var bill = new app_user_bill();
                bill.tran_type = TranType.recharge;
                bill.tran_type_source = result;
                bill.order_info = $"用户{user.username},充值金额{model.amount}";
                bill.create_time = DateTime.Now;
                bill.app_user_id = model.app_user_id;
                bill.amount = model.amount;
                model.before_funds = user.invite_funds;
                model.after_funds = user.invite_funds - model.amount;
                user.funds += model.amount;
                IAppUserService.Update(user);
                using (DbRepository db = new DbRepository())
                {
                    db.app_user_bill.Add(bill);
                    db.SaveChanges();
                }

                return JResult(new WebResult<bool> { Code = ErrorCode.sys_success, Result = true });
            }
            else
                return JResult(new WebResult<bool> { Code = ErrorCode.sys_fail, Result = false, Append = "操作失败" });
        }

        [HttpPost]
        public ActionResult Update(app_user model)
        {
            var result = IAppUserService.Manager(model,"",true,"");
            return JResult(result);
        }

        public ActionResult ChangePassword(string oldPassword, string newPassword, string cfmPassword, int id)
        {
            return JResult(IAppUserService.ChangePassword(oldPassword, newPassword, cfmPassword, id));
        }
    }
}