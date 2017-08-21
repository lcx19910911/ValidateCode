using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateCode.Model
{
    /// <summary>
    /// 提现记录
    /// </summary>
    [Table("withdrawals")]
    public class withdrawals : base_entity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        //User_ID
        [InverseProperty("UserWithdrawals")]
        public virtual app_user User { get; set; }

        [NotMapped]
        public string UserName { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? before_funds { get; set; }

        [Column(TypeName = "numeric")]
        public decimal amount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? after_funds { get; set; }

        [NotMapped]
        public string AuditUserName { get; set; }

        public AuditState audit_state { get; set; } = AuditState.Wait;
    }
    public enum AuditState
    {
        [Description("等待审核")]
        Wait =0,
        /// <summary>
        /// 转账成功
        /// </summary>
        [Description("已转账")]
        Success =1,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("已驳回")]
        Reject =2
    }
}
