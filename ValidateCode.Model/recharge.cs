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
        public int app_user_id { get; set; }

        [NotMapped]
        public string app_user_name { get; set; }

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

        public PayState order_state { get; set; } = PayState.success;
    }

    public enum PayType
    {
        alipay=0,

        wechat=1
    }

    public enum PayState
    {
        people = 0,

        waitpay =1,

       success=2,
    }
}
