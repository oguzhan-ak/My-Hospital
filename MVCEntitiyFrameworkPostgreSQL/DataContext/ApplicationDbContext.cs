using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Npgsql;
using MVCEntitiyFrameworkPostgreSQL.Models;

namespace MVCEntitiyFrameworkPostgreSQL.DataContext
{
    
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(): base(nameOrConnectionString: "Myconnection")
        {

        }

        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Hospital> hospitals { get; set; }
        public virtual DbSet<BloodTypes> bloodtypes { get; set; }
        public virtual DbSet<Doctor> doctors { get; set; }
        public virtual DbSet<Appointment> appointments { get; set; }
        public virtual DbSet<Time> times { get; set; }
        public virtual DbSet<Result> results { get; set; }
    }
}