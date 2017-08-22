
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
            var project = Find(model.id);
            if (project == null)
            {
                model.create_time = DateTime.Now;
                model.sort_index = 0;
                model.project_state = ProjectState.disable;
                int id=Add(model);
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
                project.project_state = model.project_state;
                project.send_filter_name = model.send_filter_name;
                project.inbox_filter_name = model.inbox_filter_name;
                project.price = model.price;
                project.price_discount = model.price_discount;
                project.sort_index = model.sort_index;
                project.divide_type = model.divide_type;
                project.divide_fixed_amount = model.divide_fixed_amount;
                project.divide_ratio_value = model.divide_ratio_value;
                project.stock_sum = model.stock_sum;
                project.country_code = model.country_code;
                int result=Update(project);
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
                if (!isAll)
                {
                    query = query.Where(x => x.project_state==ProjectState.enable);
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
