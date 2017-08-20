namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class project_task : base_entity
    {

        public int app_user_id { get; set; }

        public int? app_user_device { get; set; }

        public int project_id { get; set; }

        public int device_id { get; set; }

        public int? phone_id { get; set; }

        [StringLength(10)]
        public string phone_country { get; set; }

        [StringLength(20)]
        public string phone_number { get; set; }

        [StringLength(200)]
        public string phone_text { get; set; }

        [StringLength(20)]
        public string phone_text_from { get; set; }

        [StringLength(8)]
        public string phone_code { get; set; }

        public int? phone_ban { get; set; }

        [StringLength(20)]
        public string password { get; set; }

        public DateTime create_time { get; set; }

        public DateTime? access_time { get; set; }

        public DateTime? get_device_time { get; set; }

        public DateTime? get_num_time { get; set; }

        public DateTime? get_text_time { get; set; }

        public DateTime? get_resault_time { get; set; }

        public decimal? price { get; set; }

        public int payment_statu { get; set; }

        public int task_flow { get; set; }

        public DateTime? task_flow_time { get; set; }
        
    }
}
