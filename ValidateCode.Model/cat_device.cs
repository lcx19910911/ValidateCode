namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_device : base_entity
    {

        public int com { get; set; }

        [StringLength(20)]
        public string imei { get; set; }

        public int phone_id { get; set; }

        [StringLength(10)]
        public string phone_country { get; set; }

        [StringLength(20)]
        public string phone_num { get; set; }

        [StringLength(20)]
        public string phone_ccid { get; set; }

        public DateTime? apply_time { get; set; }

        public int? apply_user_id { get; set; }

        public int? apply_project_id { get; set; }

        public int? apply_task_id { get; set; }

        public int apply_statu { get; set; }

        public int sms_id { get; set; }

        [Required]
        [StringLength(20)]
        public string host_pc { get; set; }

        public int? host_user_id { get; set; }

        public DateTime active_time { get; set; }
    }
}
