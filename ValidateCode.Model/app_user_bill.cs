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

        public int tran_type { get; set; }

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

        public int statu { get; set; }
    }
}
