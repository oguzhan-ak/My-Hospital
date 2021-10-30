using MVCEntitiyFrameworkPostgreSQL.DataContext;
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
    public class DoctorController : Controller
    {
        ApplicationDbContext db;
        public DoctorController()
        {
            db = new ApplicationDbContext();
        }
        public string FindDoctornameByEmail(string email)
        {
            foreach (var item in db.doctors)
            {
                if (item.email.Equals(email))
                {
                    return item.name;
                }
            }
            return "";
        }
        public int FindIdByEmail(string email)
        {
            foreach (var item in db.doctors)
            {
                if (item.email.Equals(email))
                {
                    return item.id;
                }
            }
            return -1;
        }

        public ActionResult MainPage()
        {
            return View();
        }
        
        

        // GET
        [HttpGet]
        public ActionResult DoctorCreate()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorCreate(Doctor doctor)
        {
            if (db.doctors.Any(x => x.email == doctor.email))
            {
                ViewBag.Notification = "This e-mail used before!";
                return View();
            }
            else
            {
                doctor.createdDate = DateTime.Now;
                doctor.isActive = true;

                db.doctors.Add(doctor);
                db.SaveChanges();
                Session["IdSS"] = doctor.id.ToString();
                Session["UsernameSS"] = doctor.name.ToString();
                Session["EmailSS"] = doctor.email.ToString();
                return RedirectToAction("MainPage", "Doctor");
            }
        }

        public ActionResult DoctorLogout()
        {
            Session.Clear();
            return RedirectToAction("MainPage", "Doctor");
        }

        [HttpGet]
        public ActionResult DoctorLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult DoctorLogin(Doctor doctor)
        {
            
            var checkLogin = db.doctors.Where(x => x.email.Equals(doctor.email) && x.password.Equals(doctor.password)).FirstOrDefault();
            
            if (checkLogin != null)
            {
                if (checkLogin.isAdmin == true)
                {
                    Session["EmailSS"] = doctor.email.ToString();
                    return RedirectToAction("MainPage", "Bakan");
                }
                Session["IdSS"] = FindIdByEmail(doctor.email);
                Session["UsernameSS"] = FindDoctornameByEmail(doctor.email).ToString();
                Session["EmailSS"] = doctor.email.ToString();
                return RedirectToAction("MainPage", "Doctor");
            }
            else
            {
                ViewBag.Notification = "Wrong e-mail or password";
            }
            return View();
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
            tempDoctor.hospitalsCollection = db.hospitals.ToList<Hospital>();
            tempDoctor.name = doctor.name;
            tempDoctor.email = doctor.email;
            tempDoctor.password = doctor.password;
            tempDoctor.confirmpassword = tempDoctor.password;
            db.Entry(tempDoctor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MainPage");
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
            Session.Clear();
            return RedirectToAction("DoctorIndex");
        }


        // GET: Doctor
        public ActionResult HospitalIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("DoctorLogin", "Doctor");
            }
            return View(db.hospitals.ToList());
        }
        public ActionResult DoctorIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("DoctorLogin", "Doctor");
            }
            AppointmentDoctorHospitalVM mymodel = new AppointmentDoctorHospitalVM();
            mymodel.doctors = GetDoctors();
            mymodel.hospitals = GetHospitals();
            mymodel.appointments = GetAppointments();
            mymodel.times = GetTimes();
            return View(mymodel);
        }
        public ActionResult ToMainPage()
        {

            return View("MainPage");
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
        public ActionResult AppointmentIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("DoctorLogin", "Doctor");
            }
            AppointmentDoctorHospitalVM mymodel = new AppointmentDoctorHospitalVM();
            mymodel.doctors = GetDoctors();
            mymodel.hospitals = GetHospitals();
            mymodel.appointments = GetAppointments();
            mymodel.times = GetTimes();
            mymodel.users = GetUsers();
            mymodel.results = GetResults();
            return View(mymodel);
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
        public List<Result> GetResults()
        {
            List<Result> result = new List<Result>();
            foreach (var item in db.results.OrderByDescending(a=>a.createdDate).ToList())
            {
                if (item.createdDoctorId.ToString().Equals(Session["IdSS"].ToString()))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<Appointment> GetAppointments()
        {
            List<Appointment> appointments=new List<Appointment>();
            foreach (var item in db.appointments.ToList())
            {
                if (item.doctorId.ToString().Equals(Session["IdSS"].ToString()))
                {
                    appointments.Add(item);
                }
            }
            return appointments;
        }
        public ActionResult PatientDetails(int? id)
        {
            User user = db.users.Find(id);
            user.bloodTypesCollection = db.bloodtypes.ToList();
            user.confirmpassword = user.password;
            return View(user);
        }
        // GET
        [HttpGet]
        public ActionResult AddResult(int id)
        {
            Result result = new Result();
            result.appointmentId = id;
            return View(result);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddResult(Result result)
        {
            result.createdDate = DateTime.Now;
            result.isActive = true;
            result.createdDoctorId = Int16.Parse(Session["IdSS"].ToString());
            result.updatedDoctorId = null;
            result.updatedDate = null;
            db.results.Add(result);
            db.SaveChanges();
            return RedirectToAction("ResultIndex", "Doctor");
            
        }
        // GET: Doctor
        public ActionResult ResultIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("DoctorLogin", "Doctor");
            }
            AppointmentDoctorHospitalVM mymodel = new AppointmentDoctorHospitalVM();
            mymodel.doctors = GetDoctors();
            mymodel.hospitals = GetHospitals();
            mymodel.appointments = GetAppointments();
            mymodel.times = GetTimes();
            mymodel.results = GetResults();
            mymodel.users = GetUsers();
            return View(mymodel);
        }
        public ActionResult ResultDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.results.Find(id);
            result.doctorsCollection = db.doctors.ToList();
            result.usersCollection = db.users.ToList();
            result.appointmentsCollection = db.appointments.ToList();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
        //GET
        public ActionResult EditResult(int? id)
        {

            Result result = db.results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult EditResult(Result result)
        {
            result.usersCollection = db.users.ToList();
            result.doctorsCollection = db.doctors.ToList();
            result.appointmentsCollection = db.appointments.ToList();
            result.updatedDate = DateTime.Now;
            result.updatedDoctorId = result.createdDoctorId;
            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ResultIndex");
        }
        //GET

        public ActionResult DeleteResult(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.results.Find(id);
            result.usersCollection = db.users.ToList();
            result.appointmentsCollection = db.appointments.ToList();
            result.doctorsCollection = db.doctors.ToList();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //POST
        [HttpPost, ActionName("DeleteResult")]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult DeleteResultPost(int? id)
        {
            Result result = db.results.Find(id);
            db.results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("ResultIndex");
            
        }
    }
}