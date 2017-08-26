namespace ValidateCode.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class app_user : base_entity
    {

        [Required]
        [StringLength(16)]
        public string username { get; set; }

        [Required]
        [StringLength(16)]
        public string pasword { get; set; }

        [StringLength(15)]
        public string qq { get; set; }

        [StringLength(15)]
        public string phone { get; set; }

        [StringLength(32)]
        public string email { get; set; }

        public DateTime reg_time { get; set; }

        [StringLength(50)]
        public string reg_device { get; set; }

        public DateTime? login_time { get; set; }

        [StringLength(50)]
        public string login_ip { get; set; }

        [StringLength(50)]
        public string login_device { get; set; }

        public int? vip_level { get; set; }

        [StringLength(20)]
        public string alipay_acc { get; set; }

        [StringLength(20)]
        public string alipay_name { get; set; }

        public int? discount { get; set; } = 100;

        public bool is_invite { get; set; } = false;

        [StringLength(6)]
        public string invite_code { get; set; }

        public int? invite_user_id { get; set; }
        [NotMapped]
        public string invite_user_name { get; set; }

        /// <summary>
        /// ����ƹ�
        /// </summary>
        [Column(TypeName = "numeric")]
        public decimal? invite_profit { get; set; }

        /// <summary>
        /// �����ϼ�
        /// </summary>
        [Column(TypeName = "numeric")]
        public decimal? invite_offer { get; set; }



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


        [Column(TypeName = "numeric")]
        public decimal invite_funds { get; set; } = 0;


        [Column(TypeName = "numeric")]
        public decimal funds { get; set; } = 0;
        
    }
}
