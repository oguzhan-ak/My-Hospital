using MVCEntitiyFrameworkPostgreSQL.DataContext;
using MVCEntitiyFrameworkPostgreSQL.Helper;
using MVCEntitiyFrameworkPostgreSQL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCEntitiyFrameworkPostgreSQL.Controllers
{
    public class BakanController : Controller
    {
        ApplicationDbContext db;
        public BakanController()
        {
            db = new ApplicationDbContext();
            
        }
        public ActionResult MainPage()
        {
            return View();
        }

        // GET: Bakan
        public ActionResult Index()
        {
            return View();
        }

        // GET: Doctor
        public ActionResult HospitalIndex(string sortOrder)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            List<Hospital> hospitals = db.hospitals.ToList().OrderBy(x=>x.name).ToList();
            switch (sortOrder)
            {
                case "name_desc":
                    hospitals = hospitals.OrderByDescending(hospital => hospital.name).ToList();
                    break;
                case "Address":
                    hospitals = hospitals.OrderBy(hospital => hospital.address).ToList();
                    break;
                case "address_desc":
                    hospitals = hospitals.OrderByDescending(hospital => hospital.address).ToList();
                    break;
                default:
                    hospitals = hospitals.OrderBy(hospital => hospital.name).ToList();
                    break;
            }
            return View(hospitals.ToList());
        }
        public ActionResult GetPaggedData(int pageNumber=1,int pageSize = 20)
        {
            List<Hospital> hospitals = db.hospitals.ToList().OrderBy(x => x.name).ToList();
            var pagedData = Pagination.PagedResult(hospitals, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoctorIndex()
        {
            AppointmentDoctorHospitalVM mymodel = new AppointmentDoctorHospitalVM();
            mymodel.doctors = GetDoctors();
            mymodel.hospitals = GetHospitals();
            mymodel.appointments = GetAppointments();
            mymodel.times = GetTimes();
            return View(mymodel);
        }
        // GET
        [HttpGet]
        public ActionResult HospitalCreate()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalCreate(Hospital hospital)
        {
            if (db.hospitals.Any(x => x.name == hospital.name))
            {
                ViewBag.Notification = "This hospital added before!";
                return View();
            }
            else
            {
                hospital.createdDate = DateTime.Now;
                hospital.isActive = true;
                db.hospitals.Add(hospital);
                db.SaveChanges();
                return RedirectToAction("HospitalIndex", "Bakan");
            }
        }
        //GET
        public ActionResult HospitalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hospital hospital = db.hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }
        //POST
        [HttpPost, ActionName("HospitalDelete")]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult HospitalDeletePost(int? id)
        {
            Hospital hospital = db.hospitals.Find(id);
            db.hospitals.Remove(hospital);
            db.SaveChanges();
            return RedirectToAction("HospitalIndex");
        }


        //GET
        public ActionResult HospitalEdit(int? id)
        {

            Hospital hospital = db.hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult HospitalEdit(Hospital hospital)
        {
            if (ModelState.IsValid)
            {

                db.Entry(hospital).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HospitalIndex");
            }

            return View(hospital);
        }

        public ActionResult HospitalDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hospital hospital = db.hospitals.Find(id);

            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }

        //GET
        public ActionResult DoctorEdit(int? id)
        {

            Doctor doctor = db.doctors.Find(id);
            doctor.hospitalsCollection = db.hospitals.ToList<Hospital>();
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult DoctorEdit(Doctor doctor)
        {
            Doctor tempDoctor = db.doctors.Find(doctor.id);
            tempDoctor.salary = doctor.salary;
            tempDoctor.title = doctor.title;
            tempDoctor.hospitalId = doctor.hospitalId;
            tempDoctor.confirmpassword = tempDoctor.password;
            tempDoctor.hospitalsCollection = db.hospitals.ToList<Hospital>();
            db.Entry(tempDoctor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DoctorIndex");
        }

        public ActionResult DoctorDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.doctors.Find(id);
            doctor.hospitalsCollection = db.hospitals.ToList<Hospital>();
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }
        //GET
        public ActionResult DoctorDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.doctors.Find(id);
            doctor.hospitalsCollection = db.hospitals.ToList();
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //POST
        [HttpPost, ActionName("DoctorDelete")]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult DoctorDeletePost(int? id)
        {
            Doctor doctor = db.doctors.Find(id);
            db.doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("DoctorIndex");
        }

        public ActionResult BakanLogout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = db.doctors.ToList<Doctor>();
            return doctors;
        }
        public List<User> GetUsers()
        {
            List<User> users = db.users.ToList<User>();
            return users;
        }
        public List<Time> GetTimes()
        {
            List<Time> times = db.times.ToList<Time>();
            return times;
        }
        public List<Hospital> GetHospitals()
        {
            List<Hospital> hospital = db.hospitals.ToList<Hospital>();
            return hospital;
        }
        public List<Appointment> GetAppointments()
        {
            List<Appointment> appointments = db.appointments.ToList();
            return appointments;
        }
    }
}