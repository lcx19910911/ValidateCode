
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
    public interface IAdminUserService : IBaseService<admin_user>
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<admin_user> GetPageList(int pageIndex, int pageSize, string account);

        WebResult<admin_user> Login(string account, string password,string code);

        WebResult<bool> Manager(admin_user model);
        WebResult<bool> ChangePassword(string oldPassword, string newPassword, string cfmPassword, int id);

    }
}
