using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebArchiwum.WebUI.Models;
using Domain.Utils;
using Domain.Core;
using Newtonsoft.Json;
using System.Web.Security;
using WebArchiwum.WebUI.Service;
using WebArchiwum.WebUI.Interface;

namespace WebArchiwum.WebUI.Controllers
{
    
    public class AdminController : Controller
    {
        IUserService userService = null;
        public AdminController()
        {
            userService = new UserService();
        }

        MyDbContext db = new MyDbContext();
        UserUtils userUtils = new UserUtils();
        //
        // GET: /Admin/
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User model)
        {
            
            int userId = 0;
            try
            {
                userId = userService.Login(model);
            }
            catch (Exceptions.AccountNotActivatedException)
            {
                return RedirectToAction("Index", "Error");
                //return Json(JsonReturns.Redirect("/User/AccountNotActivated"), JsonRequestBehavior.AllowGet);
            }

            var authTicket = new FormsAuthenticationTicket(
                1,
                userId.ToString(),
                DateTime.Now,
                DateTime.Now.AddMinutes(1440),
                true,
                "",
                FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(authCookie);

            return RedirectToAction("YearView","Admin");
        }

        
        [Authorize]
        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {

                if (user.UserId == 0 && db.Users.Where(p=>p.Login.ToLower() == user.Login.ToLower()).Count()==0)
                {
                    user.Password = userUtils.PasswordHash(user.Password);
                    var register = db.Users.Create();
                    register.UserId = user.UserId;
                    register.Login = user.Login;
                    register.Password = user.Password;
                    register.ConfirmPassword = user.Password;

                    db.Users.Add(register);
                }
                else
                {
                    ViewBag.Blad = "Login taki jest już zajęty";
                    return View(user);
                }
                db.SaveChanges();
                TempData["message"] = string.Format("Zapisano {0} ", user.Login);
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View(user);
            }

        }
        [Authorize]
        public ViewResult YearView()
        {
            var model = from c in db.Set<Year>()
                        select c;
            ViewBag.a = userService.GetCurrentUser(User).Login;
            return View(model);
        }
        [Authorize]
        public ViewResult YearAdd()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult YearAdd(Year year)
        {
            if (ModelState.IsValid)
            {

                if (year.YearId == 0)
                {
                    db.Set<Year>().Add(year);
                }
                else
                {
                    Year dbEntry = db.Set<Year>().Find(year.YearId);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = year.Name;

                    }
                }
                db.SaveChanges();
                TempData["message"] = string.Format("Zapisano {0} ", year.Name);
                return RedirectToAction("YearView","Admin");
            }
            else
            {
                return View(year);
            }
        }
        [Authorize]
        public ViewResult YearEdit(int YearId)
        {
            Year model = db.Set<Year>().ToList()
                .SingleOrDefault(p => p.YearId == YearId);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult YearEdit(Year year)
        {
            if (ModelState.IsValid)
            {

                if (year.YearId == 0)
                {
                    db.Set<Year>().Add(year);
                }
                else
                {
                    Year dbEntry = db.Set<Year>().Find(year.YearId);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = year.Name;

                    }
                }
                db.SaveChanges();
                TempData["message"] = string.Format("Zapisano {0} ", year.Name);
                return RedirectToAction("YearView", "Admin");
            }
            else
            {
                return View(year);
            }
        }
        [Authorize]
        public ViewResult GraduateView(int yearId)
        {
            ViewBag.yearr = ViewBag.year;
            ViewBag.id = yearId;
            var model = from o in db.Set<Year>()
                        join o2 in db.Set<Graduate>()
                        on o.YearId equals o2.YearId
                        where o.YearId.Equals(yearId)
                        select new YearAndGraduate { Years = o, Graduates = o2 };

            return View(model);
        }
        [Authorize]
        public ViewResult GraduateAdd(int? YearId)
        {
            var model = new YearAndGraduate { Graduates = new Graduate { YearId = YearId } };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult GraduateAdd(YearAndGraduate graduate, int? YearId)
        {

            if (ModelState.IsValid)
            {

                if (graduate.Graduates.GraduateId == 0)
                {

                    db.Set<Graduate>().Add(graduate.Graduates);
                    YearId = graduate.Graduates.YearId;
                }
                else
                {
                    Graduate dbEntry = db.Set<Graduate>().Find(graduate.Graduates.GraduateId);
                    if (dbEntry != null)
                    {
                        dbEntry.FirstName = graduate.Graduates.FirstName;
                        dbEntry.LastName = graduate.Graduates.LastName;
                        dbEntry.BIO = graduate.Graduates.BIO;
                        dbEntry.YearId = graduate.Graduates.YearId;

                    }
                }
                db.SaveChanges();
                TempData["message"] = string.Format("Zapisano {0} ", graduate.Graduates.FirstName +" "+ graduate.Graduates.LastName);
                return RedirectToAction("GraduateView","Admin", new { YearId });
            }
            else
            {
                return View(graduate);
            }
        }
        [Authorize]
        public ViewResult GraduateEdit(int GraduateId)
        {
            
            Graduate model = db.Set<Graduate>().ToList()
                .SingleOrDefault(p => p.GraduateId == GraduateId);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult GraduateEdit(Graduate graduate, int? YearId)
        {

            if (ModelState.IsValid)
            {

                if (graduate.GraduateId == 0)
                {
                    db.Set<Graduate>().Add(graduate);
                }
                else
                {
                    Graduate dbEntry = db.Set<Graduate>().Find(graduate.GraduateId);
                    if (dbEntry != null)
                    {
                        dbEntry.FirstName = graduate.FirstName;
                        dbEntry.LastName = graduate.LastName;
                        dbEntry.BIO = graduate.BIO;

                    }
                }
                db.SaveChanges();
                TempData["message"] = string.Format("Zapisano {0} ", graduate.FirstName +" "+ graduate.LastName);
                return RedirectToAction("GraduateView", "Admin", new { YearId });
            }
            else
            {
                return View(graduate);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult GraduateDelete(int GraduateId)
        {

            Graduate dbEntry = db.Set<Graduate>().Find(GraduateId);
            var yearId = dbEntry.YearId;
            if (dbEntry != null)
            {
                db.Set<Graduate>().Remove(dbEntry);
                db.SaveChanges();
                TempData["message"] = string.Format("Usunięto {0}",  dbEntry.FirstName+dbEntry.LastName);
            }

            return RedirectToAction("GraduateView","Admin", new { yearId });
            
        }
        [Authorize]
        [HttpPost]
        public ActionResult YearDelete(int yearId)
        {
            var model = from o in db.Set<Year>()
                        join o2 in db.Set<Graduate>()
                        on o.YearId equals o2.YearId
                        where o.YearId.Equals(yearId)
                        select new YearAndGraduate { Years = o, Graduates = o2 };
            if (model.Count() > 0)
            {
                return View("YearErrorView");
            }
            else
            {
                Year dbEntry = db.Set<Year>().Find(yearId);
                if (dbEntry != null)
                {
                    db.Set<Year>().Remove(dbEntry);
                    db.SaveChanges();
                    TempData["message"] = string.Format("Usunięto {0}", dbEntry.Name);
                }
                return RedirectToAction("YearView", "Admin");
            }
        }

        public ActionResult YearErrorView()
        {
            return View();
        }


    }
}
