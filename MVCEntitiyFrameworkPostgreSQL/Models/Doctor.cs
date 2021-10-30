using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("Doctor", Schema = "public")]
    public class Doctor
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Doctor Name")]
        [Required(ErrorMessage = "Doctor name can't be empty!")]
        public string name { get; set; }
        [Required(ErrorMessage = "E-Mail can't be empty!")]
        [DisplayName("E-Mail")]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password can't be empty!")]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("password", ErrorMessage = "This password doesn't matching with other one!")]
        [Required(ErrorMessage = "Password can't be empty!")]
        [NotMapped]
        public string confirmpassword { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime createdDate { get; set; }

        public Boolean isActive { get; set; }

        [DisplayName("Hospital Name")]
        public int? hospitalId { get; set; }
        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Salary")]
        public int? salary { get; set; }

        [NotMapped]
        public List<Hospital> hospitalsCollection { get; set; }
        [DisplayName("Are you admin?")]
        public Boolean isAdmin { get; set; }

    }
}