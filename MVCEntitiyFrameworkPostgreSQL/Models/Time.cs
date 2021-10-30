using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("Time", Schema = "public")]
    public class Time
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Hour")]
        [Required(ErrorMessage = "Hour can't be empty!")]
        public string time { get; set; }
    }
}