
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
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<User> GetPageList(int pageIndex, int pageSize, string account,string phone, UserType? type);

        WebResult<User> Login(string account, string password,string code);

        WebResult<bool> Manager(User model);
        WebResult<bool> ChangePassword(string oldPassword, string newPassword, string cfmPassword, int id);

    }
}
