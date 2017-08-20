namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("phone")]
    public partial class phone : base_entity
    {

        [Required]
        [StringLength(10)]
        public string country { get; set; }

        [StringLength(20)]
        public string num { get; set; }

        [StringLength(20)]
        public string ccid { get; set; }

        [StringLength(20)]
        public string imsi { get; set; }

        public DateTime record_time { get; set; }
    }
}
