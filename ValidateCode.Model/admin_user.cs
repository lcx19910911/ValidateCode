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
        /// ����������
        /// </summary>
        [Display(Name = "����������")]
        [MaxLength(12), MinLength(6)]
        [NotMapped]
        public string new_password { get; set; }

        /// <summary>
        /// �ٴ���������
        /// </summary>
        [MaxLength(12), MinLength(6), Compare("new_password", ErrorMessage = "�����������벻һ��")]
        [NotMapped]
        public string confirm_password { get; set; }
    }

    public enum AdminType
    {
        normal=0,
        super=1,
    }
}
