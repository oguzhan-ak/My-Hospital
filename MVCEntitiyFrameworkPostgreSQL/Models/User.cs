using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("User", Schema = "public")]
    public class User
    {
        private const string V = "This field is required";

        [Key]
        public int id { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User name can't be empty!")]
        public string username { get; set; }
        [Required(ErrorMessage = "E-Mail can't be empty!")]
        [DisplayName("E-Mail")]
        [DataType(DataType.EmailAddress)]
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
        [DisplayName("Age")]
        public int? age { get; set; }
        [DisplayName("Weight (kg)")]
        public double? weight { get; set; }
        [DisplayName("Height (cm)")]
        public double? height { get; set; }
        
        [DisplayName("Blood Type")]
        public int bloodTypeId { get; set; }

        [NotMapped]
        public List<BloodTypes> bloodTypesCollection { get; set; }

    }
}