using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("BloodTypes", Schema = "public")]
    public class BloodTypes
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Blood Type")]
        public string type { get; set; }
    }
}