﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValidateCode.Core.Model;
using ValidateCode.IService;
using ValidateCode.Model;

namespace ValidateCode.IService
{
    public interface IRechargeService : IBaseService<recharge>
    {

        //                充值
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<recharge> GetPageList(int pageIndex, int pageSize,string name,  DateTime? createdTimeStart, DateTime? createdTimeEnd, int userId=0);

    }
}
