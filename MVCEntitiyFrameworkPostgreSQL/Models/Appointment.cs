using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("Appointment", Schema = "public")]
    public class Appointment
    {
        [Key]
        public int id { get; set; }
        [DisplayName("E mail")]
        public int? userId { get; set; }
        [DisplayName("Doctor Name")]
        [Required(ErrorMessage = "Doctor name can't be empty!")]
        public int? doctorId { get; set; }
        [DisplayName("Hospital Name")]
        [Required(ErrorMessage = "Hospital name can't be empty!")]
        public int? hospitalId { get; set; }
        [DisplayName("Hour")]
        [Required(ErrorMessage = "Hour can't be empty!")]
        public int? timeId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("Appointment Date")]
        [Required(ErrorMessage = "Appointment date can't be empty!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime appointmentDate { get; set; }
        
        [DisplayName("Appointment Note")]
        [Required(ErrorMessage = "Appointment note can't be empty!")]
        public string appointmentNote { get; set; }
        public Boolean isActive { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime createdDate { get; set; }
        [NotMapped]
        public List<Doctor> doctorsCollection { get; set; }
        [NotMapped]
        public List<Hospital> hospitalsCollection { get; set; }
        [NotMapped]
        public List<Time> timesCollection { get; set; }


    }
}