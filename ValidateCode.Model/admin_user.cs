namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class admin_user:base_entity
    {

        [Required]
        [StringLength(32)]
        public string account { get; set; }
        [Required]
        [StringLength(256)]
        public string password { get; set; }
        [Required]
        public AdminType type { get; set; }

        [Required]
        public DateTime created_time { get; set; }


        /// <summary>
        /// 请输入密码
        /// </summary>
        [Display(Name = "请输入密码")]
        [MaxLength(12), MinLength(6)]
        [NotMapped]
        public string new_password { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        [MaxLength(12), MinLength(6), Compare("new_password", ErrorMessage = "两次密码输入不一致")]
        [NotMapped]
        public string confirm_password { get; set; }
    }

    public enum AdminType
    {
        normal=0,
        super=1,
    }
}
