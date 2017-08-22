namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class app_user_bill : base_entity
    {

        public int app_user_id { get; set; }
        [NotMapped]
        public string app_user_name { get; set; }

        public TranType tran_type { get; set; }

        public int? tran_type_source { get; set; }

        [Required]
        [StringLength(200)]
        public string order_info { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? before_funds { get; set; }

        [Column(TypeName = "numeric")]
        public decimal amount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? after_funds { get; set; }

        public DateTime create_time { get; set; }

        [StringLength(32)]
        public string third_order_id { get; set; }

        public AuditState audit_state { get; set; } = AuditState.success;

        public DateTime? audit_time { get; set; }

        public PayType? type { get; set; } = PayType.people;

    }



    //1-用户充值  2-接收短信 3-发送短信  4-扣款  5-退款  6-提现  7-推广提成
    public enum TranType
    {
        recharge=1,

        reviceSms=2,

        sendSms=3,

        debit=4,

        returnPay=5,

        withdrawls=6,

        invate=7,

    }

    public enum PayType
    {
        people=0,

        alipay=1,

        wechat=2,
    }

    public enum AuditState
    {
        wait=0,

        success=1,

        reject=2,
    }
}
