namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class invite_bill : base_entity
    {

        public int app_user_id { get; set; }

        public int from_user_id { get; set; }

        [Required]
        [StringLength(16)]
        public string from_user_name { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string description { get; set; }

        [Column(TypeName = "numeric")]
        public decimal amount { get; set; }

        public int project_id { get; set; }

        public int task_type { get; set; }

        public int task_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal task_cost { get; set; }

        public int verify_statu { get; set; }

        public DateTime? verify_time { get; set; }

        public DateTime create_time { get; set; }
    }
}
