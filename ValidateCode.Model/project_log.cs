namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class project_log : base_entity
    {

        public int project_id { get; set; }

        public int? user_id { get; set; }

        public int phone_id { get; set; }

        public int? block_type { get; set; }

        public DateTime record_time { get; set; }
    }
}
