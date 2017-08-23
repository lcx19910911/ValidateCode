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
        
        public int app_user_id { get; set; }

        [NotMapped]
        public string app_user_name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? before_funds { get; set; }

        [Column(TypeName = "numeric")]
        public decimal amount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? after_funds { get; set; }


        public DateTime create_time { get; set; }

        public AuditState audit_state { get; set; } = AuditState.wait;
        public PayType type { get; set; } = PayType.alipay;
        public DateTime? audit_time { get; set; }

        [MaxLength(32)]
        public string third_order_id { get; set; }


    }
    public enum AuditState
    {
        [Description("等待审核")]
        wait =0,
        /// <summary>
        /// 转账成功
        /// </summary>
        [Description("已转账")]
        success =1,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("已驳回")]
        reject =2
    }
}
