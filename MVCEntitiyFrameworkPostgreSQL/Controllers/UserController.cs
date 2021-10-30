using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEntitiyFrameworkPostgreSQL.Models;
using MVCEntitiyFrameworkPostgreSQL.DataContext;
using System.Net;
using System.Data.Entity;
using System.Dynamic;

namespace MVCEntitiyFrameworkPostgreSQL.Controllers
{
    public class UserController : Controller
    {

        ApplicationDbContext db;
        public UserController()
        {
            db = new ApplicationDbContext();
        }

        public string FindUsernameByEmail(string email)
        {
            foreach(var item in db.users)
            {
                if (item.email.Equals(email))
                {
                    return item.username;
                }
            }
            return "";
        }
        public int FindIdByEmail(string email)
        {
            foreach (var item in db.users)
            {
                if (item.email.Equals(email))
                {
                    return item.id;
                }
            }
            return -1;
        }
        
        public ActionResult ToMainPage()
        {
            
            return View("MainPage");
        }

        // GET: User
        public ActionResult Index()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("Create", "User");
            }
            return View(db.users.ToList());
        }
        public ActionResult MainPage()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            user.bloodTypesCollection = db.bloodtypes.ToList<BloodTypes>();
            
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        

        //GET
        public ActionResult Edit(int? id)
        {
            
            User user = db.users.Find(id);
            user.bloodTypesCollection = db.bloodtypes.ToList<BloodTypes>();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(user);
        }

        //GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult DeletePost(int? id)
        {
            User user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");
        }

        // GET
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if(db.users.Any(x=>x.email == user.email))
            {
                ViewBag.Notification = "This e-mail used before!";
                return View();
            }else if(db.users.Any(x => x.username == user.username))
            {
                ViewBag.Notification = "This username used before!";
                return View();
            }
            else
            {
                user.createdDate = DateTime.Now;
                user.isActive = true;
                user.bloodTypeId = -1;
                db.users.Add(user);
                db.SaveChanges();
                Session["IdSS"] = user.id.ToString();
                Session["UsernameSS"] = user.username.ToString();
                Session["EmailSS"] = user.email.ToString();
                return RedirectToAction("MainPage", "User");
            }
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("MainPage", "User");
        }

        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var checkLogin = db.users.Where(x => x.email.Equals(user.email) && x.password.Equals(user.password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["IdSS"] = FindIdByEmail(user.email);
                Session["UsernameSS"] = FindUsernameByEmail(user.email).ToString();
                Session["EmailSS"] = user.email.ToString();
                return RedirectToAction("MainPage", "User");
            }
            else
            {
                ViewBag.Notification = "Wrong e-mail or password";
            }
            return View();
        }

        // GET: Doctor
        public ActionResult HospitalIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View(db.hospitals.ToList());
        }
        public ActionResult DoctorIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            AppointmentDoctorHospitalVM mymodel = new AppointmentDoctorHospitalVM();
            mymodel.doctors = GetDoctors();
            mymodel.hospitals = GetHospitals();
            mymodel.appointments = GetAppointments();
            mymodel.times = GetTimes();
            return View(mymodel);
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
        
        [HttpGet]
        public ActionResult TakeAppointment()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            Appointment appointment = new Appointment();
            appointment.hospitalsCollection = db.hospitals.ToList();
            appointment.doctorsCollection = db.doctors.ToList();
            appointment.timesCollection = db.times.ToList();
            appointment.appointmentDate = DateTime.Now;
            return View(appointment);

        }
        [HttpPost]
        public ActionResult TakeAppointment(Appointment appointment)
        {
            appointment.createdDate = DateTime.Now;
            appointment.isActive = true;
            appointment.userId = (int)Session["IdSS"];
            appointment.hospitalsCollection = db.hospitals.ToList();
            appointment.doctorsCollection = db.doctors.ToList();
            appointment.timesCollection = db.times.ToList();
            db.appointments.Add(appointment);
            db.SaveChanges();
            return RedirectToAction("MainPage", "User");
            
        }
        public JsonResult GetDoctorList(int hospitalId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Doctor> doctorList = db.doctors.Where(x => x.hospitalId == hospitalId).ToList();
            return Json(doctorList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppointmentIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            AppointmentDoctorHospitalVM mymodel = new AppointmentDoctorHospitalVM();
            mymodel.doctors = GetDoctors();
            mymodel.hospitals = GetHospitals();
            mymodel.appointments = GetAppointments();
            mymodel.times = GetTimes();
            mymodel.results = GetResults();
            return View(mymodel);
        }
        public List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = db.doctors.ToList<Doctor>();
            return doctors;
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
        public List<User> GetUsers()
        {
            List<User> users = db.users.ToList<User>();
            return users;
        }
        public List<Result> GetResults()
        {
            List<Result> result = new List<Result>();
            List<Appointment> appointments = new List<Appointment>();
            foreach (var item1 in db.appointments.OrderByDescending(x=>x.createdDate).ToList())
            {
                if (item1.userId.ToString().Equals(Session["IdSS"].ToString()))
                {
                    appointments.Add(item1);
                }
            }
            foreach (var item2 in db.results.ToList())
            {
                foreach (var item3 in appointments)
                {
                    if (item2.appointmentId.ToString().Equals(item3.id.ToString()))
                    {
                        result.Add(item2);
                    }
                }
                
            }
            return result;
        }
        public List<Appointment> GetAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (var item in db.appointments.ToList())
            {
                if (item.userId.ToString().Equals(Session["IdSS"].ToString()))
                {
                    appointments.Add(item);
                }
            }
            appointments = appointments.OrderBy(item => item.appointmentDate).ThenBy(item=>item.timeId).ToList();
            return appointments;
        }

        //GET

        public ActionResult AppointmentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.appointments.Find(id);
            appointment.hospitalsCollection = db.hospitals.ToList();
            appointment.timesCollection = db.times.ToList();
            appointment.doctorsCollection = db.doctors.ToList();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        //POST
        [HttpPost, ActionName("AppointmentDelete")]
        [ValidateAntiForgeryToken]//güvenlik için
        public ActionResult AppointmentDeletePost(int? id)
        {
            Appointment appointment = db.appointments.Find(id);
            db.appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("AppointmentIndex");
        }
        public ActionResult AppointmentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.appointments.Find(id);
            appointment.hospitalsCollection = db.hospitals.ToList();
            appointment.doctorsCollection = db.doctors.ToList();
            appointment.timesCollection = db.times.ToList();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
        // GET: Doctor
        public ActionResult ResultIndex()
        {
            if (Session["IdSS"] == null)
            {
                return RedirectToAction("Login", "User");
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
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
    }
}