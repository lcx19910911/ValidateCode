namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sms_inbox : base_entity
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
        public string phone_country { get; set; }

        [Required]
        [StringLength(20)]
        public string phone_num { get; set; }

        [Required]
        [StringLength(200)]
        public string phone_text { get; set; }

        [StringLength(30)]
        public string phone_text_from { get; set; }

        [Column(TypeName = "text")]
        public string phone_text_pdu { get; set; }

        public DateTime receive_time { get; set; }

        public DateTime record_time { get; set; }

        public int read_statu { get; set; }

        public int bill_statu { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price_discount { get; set; }
        
    }
}
