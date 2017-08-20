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
    [Table("Withdrawals")]
    public class Withdrawals : BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        //User_ID
        [InverseProperty("UserWithdrawals")]
        public virtual User User { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// 审核用户id
        /// </summary>
        //AuditUser_ID
        [InverseProperty("AuditWithdrawals")]

        public User AuditUser { get; set; }
        [NotMapped]
        public string AuditUserName { get; set; }

        public AuditState State { get; set; } = AuditState.Wait;

        [NotMapped]
        public string StateStr { get; set; }

        /// <summary>
        /// 转账 微信订单号
        /// </summary>
        [MaxLength(32)]
        public string VoucherNo { get; set; }

        /// <summary>
        /// 转账截图
        /// </summary>
        [MaxLength(128)]
        public string VoucherImg { get; set; }


        /// <summary>
        /// 支付宝姓名
        /// </summary>
        [Display(Name = "支付宝姓名"), MaxLength(32)]
        [Required]
        public string AliPayName { get; set; }
        /// <summary>
        /// 支付宝账号
        /// </summary>
        [Display(Name = "支付宝账号"), MaxLength(32)]
        [Required]
        public string AliPayAccount { get; set; }
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
        /// 管理员取消
        /// </summary>
        [Description("管理员取消")]
        AdminCancle =2,


        /// <summary>
        /// 用户自己取消
        /// </summary>
        [Description("用户自己取消")]
        SelfCancle = 3
    }
}
