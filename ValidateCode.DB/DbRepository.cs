using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ValidateCode.Core.Util;
using ValidateCode.Model;

namespace ValidateCode.DB
{
    public class DbRepository : DataBaseMdf
    {



        public override int SaveChanges()
        {
            try
            {
                var entries = from e in this.ChangeTracker.Entries()
                              where e.State != EntityState.Unchanged
                              select e;   //过滤所有修改了的实体，包括：增加 / 修改 / 删除
                if (entries.Count() == 0)
                    return 1;
                else
                    return base.SaveChanges();

            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                //并发冲突数据
                if (ex.GetType() == typeof(DbUpdateConcurrencyException))
                {
                    return -1;
                }
                return 0;
            }

        }
        
    }

}
