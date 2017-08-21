﻿
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ValidateCode.Core;
using ValidateCode.Core.Code;
using ValidateCode.Core.Extensions;
using ValidateCode.Core.Helper;
using ValidateCode.Core.Model;
using ValidateCode.Core.Web;
using ValidateCode.DB;
using ValidateCode.IService;
using ValidateCode.Model;

namespace ValidateCode.Service
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class ProjectService : BaseService<project>, IProjectService
    {
        public ProjectService()
        {
            base.ContextCurrent = HttpContext.Current;
        }


        /// <summary>
        /// 编辑管理用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Manager(project model)
        {
            var user = Find(model.id);
            if (user == null)
            {
                model.create_time = DateTime.Now;
                model.sort_index = 0;
                int id=Add(model);
                int sortIndex = 0;
                if (id > 0)
                {
                    model.sort_index = id;
                    Update(model);
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }
            else
            {

                if (IsExits(x => x.username == model.username && x.id != model.id))
                    return Result(false, ErrorCode.user_account_already_exist);

                user.alipay_acc = model.alipay_acc;
                user.alipay_name = model.alipay_name;
                user.qq = model.qq;
                user.phone = model.phone;
                user.email = model.email;
                if (model.vip_level != 0)
                {
                    user.vip_level = model.vip_level;
                }
                int result=Update(user);
                if (result > 0)
                {
                    return Result(true);
                }
                else
                    return Result(false, ErrorCode.sys_fail);
            }

        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<project> GetPageList(int pageIndex, int pageSize, string name, bool isAll = false)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.project.Where(x => x.statu != EntityStatu.delete);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.name.Contains(name));
                }
                if (isAll)
                {

                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.sort_index).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                list.ForEach(x =>
                {
                    if (x != null)
                    {
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);

            }
        }

        


    }
}
