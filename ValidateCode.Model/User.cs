using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ValidateCode.Model
{

    /// <summary>
    /// 微信用户
    /// </summary>
    [Table("User")]
    public partial class User : BaseEntity
    {

        [Display(Name = "账号"), MaxLength(32)]
        public string Account { get; set; }

        [Display(Name = "密码"), MaxLength(32)]
        public string Password { get; set; }
        /// <summary>
        /// 请输入密码
        /// </summary>
        [Display(Name = "请输入密码")]
        [MaxLength(12), MinLength(6)]
        [NotMapped]
        public string NewPassword { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        [MaxLength(12), MinLength(6), Compare("NewPassword", ErrorMessage = "两次密码输入不一致")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType Type { get; set; } = UserType.User;

        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码"), MaxLength(32)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "QQ"), MaxLength(32)]
        public string QQ { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱"), MaxLength(126)]
        public string Email { get; set; }


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

        public decimal Balance { get; set; }

        /// <summary>
        /// 充值总额
        /// </summary>
        [Display(Name = "充值总额")]
        public int TotalRecharge { get; set; }
        [JsonIgnore]
        public virtual List<Recharge> UserRecharges { get; set; }
        [JsonIgnore]
        public virtual List<Recharge> CreateRecharges { get; set; }

        /// <summary>
        /// 提现总额
        /// </summary>
        [Display(Name = "提现总额")]
        public int TotalWithdrawals { get; set; }
        [JsonIgnore]
        public virtual List<Withdrawals> UserWithdrawals { get; set; }
        [JsonIgnore]
        public virtual List<Withdrawals> AuditWithdrawals { get; set; }
    }

    public enum UserType
    {
        User = 1,

        Admin = 2,
    }
}

