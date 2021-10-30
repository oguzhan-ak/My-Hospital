using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCEntitiyFrameworkPostgreSQL.Models
{
    public class AppointmentDoctorHospitalVM
    {
        public IEnumerable<Doctor> doctors { get; set; }
        public IEnumerable<Hospital> hospitals { get; set; }

        public IEnumerable<Appointment> appointments { get; set; }
        public IEnumerable<Time> times { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<Result> results { get; set; }
    }
}