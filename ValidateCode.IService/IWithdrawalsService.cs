﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValidateCode.Core.Model;
using ValidateCode.Model;

namespace ValidateCode.IService
{
    public interface IWithdrawalsService : IBaseService<app_user_bill>
    {

        //                           提现记录
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<app_user_bill> GetPageList(int pageIndex, int pageSize, string name,  DateTime? createdTimeStart, DateTime? createdTimeEnd, int userId=0);

        WebResult<bool> Audit(int id, AuditState state, PayType? type, string orderId);
    }
}
