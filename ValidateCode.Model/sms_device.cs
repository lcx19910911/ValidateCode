namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sms_device : base_entity
    {

        [Required]
        [StringLength(50)]
        public string device_type { get; set; }

        [Required]
        [StringLength(50)]
        public string device_name { get; set; }

        [StringLength(200)]
        public string device_desc { get; set; }

        [Column(TypeName = "text")]
        public string config { get; set; }

        [StringLength(50)]
        public string host_pc { get; set; }

        public DateTime create_time { get; set; }
        
    }
}
