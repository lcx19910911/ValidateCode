namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("project")]
    public partial class project : base_entity
    {

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(200)]
        public string description { get; set; }

        [Column(TypeName = "numeric")]
        public decimal price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal price_discount { get; set; }

        [StringLength(5)]
        public string country_code { get; set; }

        public int stock_sum { get; set; }

        public int sort_index { get; set; }

        public int? payment_type { get; set; }

        public DateTime? create_time { get; set; }

        public DateTime? last_modify_time { get; set; }

        [StringLength(50)]
        public string inbox_filter_name { get; set; }

        [StringLength(50)]
        public string send_filter_name { get; set; }

        public int? server_statu { get; set; }
        
    }
}
