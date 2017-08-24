namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_token : base_entity
    {

        public int user_id { get; set; }

        [Required]
        [StringLength(32)]
        public string token { get; set; }

        public DateTime create_time { get; set; }

        public DateTime access_time { get; set; }

        public DateTime expire_time { get; set; }

        [StringLength(32)]
        public string device { get; set; }

        public int device_type { get; set; } = 0;

        [StringLength(32)]
        public string device_ip { get; set; }

        [StringLength(200)]
        public string device_info { get; set; }
        
    }
}
