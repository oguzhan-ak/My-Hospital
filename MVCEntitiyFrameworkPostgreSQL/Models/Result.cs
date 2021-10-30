using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    [Table("Results", Schema = "public")]
    public class Result
    {
        [Key]
        public int id { get; set; }
        public int appointmentId { get; set; }
        [DisplayName("Result")]
        [Required(ErrorMessage = "Result can't be empty!")]
        public string resultText { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("created Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime createdDate { get; set; }
        public Boolean isActive { get; set; }
        public int createdDoctorId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("updated Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? updatedDate { get; set; }
        public int? updatedDoctorId { get; set; }
        [NotMapped]
        public List<Doctor> doctorsCollection { get; set; }
        [NotMapped]
        public List<User> usersCollection { get; set; }
        [NotMapped]
        public List<Appointment> appointmentsCollection { get; set; }
    }
}