using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("Hospital", Schema = "public")]
    public class Hospital
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Hospital Name")]
        [Required(ErrorMessage = "Hospital name can't be empty!")]
        public string name { get; set; }
        [DisplayName("Hospital Address")]
        [Required(ErrorMessage = "Hospital address can't be empty!")]
        public string address { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdDate { get; set; }

        public Boolean isActive { get; set; }
    }
}