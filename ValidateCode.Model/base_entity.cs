using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateCode.Model
{
    public class base_entity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public EntityStatu statu { get; set; } = EntityStatu.normal;
    }

    public enum EntityStatu
    {
        normal=1100,
        delete=-1,
        locked=1102,
    }

}
