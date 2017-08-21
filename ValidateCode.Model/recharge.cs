using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateCode.Model
{
    /// <summary>
    /// 充值记录
    /// </summary>
    [Table("recharge")]
    public class recharge : base_entity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        // User_ID
        [InverseProperty("UserRecharges")]
        public virtual app_user User { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? before_funds { get; set; }

        [Column(TypeName = "numeric")]
        public decimal amount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? after_funds { get; set; }

        [MaxLength(32)]
        public string third_order_id { get; set; }

        public DateTime create_time { get; set; }

        public PayType type { get; set; } = PayType.alipay;

        public PayStatu order_state { get; set; } = PayStatu.waitpay;
    }

    public enum PayType
    {
        alipay=0,

        wechat=1
    }

    public enum PayStatu
    {
       waitpay=0,

       success=1,
    }
}
