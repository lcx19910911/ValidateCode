namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sms_send : base_entity
    {

        public int? app_user_id { get; set; }
        [NotMapped]
        public string app_user_name { get; set; }

        public int? project_id { get; set; }

        [StringLength(50)]
        public string project_name { get; set; }

        public int? task_id { get; set; }

        public int? device_id { get; set; }

        public int phone_id { get; set; }

        [Required]
        [StringLength(10)]
        public string text_country { get; set; }

        [Required]
        [StringLength(20)]
        public string text_num { get; set; }

        [Required]
        [StringLength(200)]
        public string text_msg { get; set; }

        public DateTime create_time { get; set; }

        public DateTime? send_time { get; set; }

        public int? send_resault { get; set; }

        public int? bill_statu { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price_discount { get; set; }

    }
}
