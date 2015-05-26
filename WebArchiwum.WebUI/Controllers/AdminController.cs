using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebArchiwum.WebUI.Models;
using Domain.Utils;
using Domain.Core;
using Domain.Security;
using Newtonsoft.Json;
using System.Web.Security;

namespace WebArchiwum.WebUI.Controllers
{
    public class AdminController : BaseController
    {

        MyDbContext db = new MyDbContext();
        UserUtils userUtils = new UserUtils();
        //
        // GET: /Admin/

        public ViewResult Login()
        {
            return View();
        }

    
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            //Sprawdza walidacje danych po stornie db.
            if (ModelState.IsValid)
            {   //Pobiera użytkowniak
                var password = userUtils.PasswordHash(model.Password);
                var user = db.Users.Where(u => u.Login == model.Username && u.Password == password).FirstOrDefault();
                if (user != null)
                {   //Sprawdza role
                    //var roles = user.Roles.Select(m => m.RoleName).ToArray();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    serializeModel.FirstName = user.Login;
                    serializeModel.LastName = user.Password;
                    //serializeModel.IsActive = user.IsActive;
                    //serializeModel.roles = roles;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             "Login",
                             DateTime.Now,
                             DateTime.Now.AddDays(1),
                             false,
                             userData);
                  
                    //Ustawia ciasteczko
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    if(user.Login != null)
                    {
                        return RedirectToAction("Index", "Test");
                    }
                    else
                    {
                        return RedirectToAction("AccountDenied", "Error");
                    }
                    //Sprawdza aktywność użytkownika
                    //if (user.IsActive == true)
                    //{   //Sprawdza pierwsze logowanie
                    //    if (user.IsChange == true )
                    //    {   //Sprawdza 30 dni do zmiany hasła
                    //        if (DateTime.UtcNow.DayOfYear - user.ExpirationDate.DayOfYear < 30)
                    //        {   
                    //            if (roles.Contains("Admin"))
                    //            {
                    //                return RedirectToAction("ViewUsers", "Administrator");
                    //            }
                    //            else if (roles.Contains("Umowa"))
                    //            {
                    //                return RedirectToAction("Index", "Contract");
                    //            }
                    //            else if (roles.Contains("Zgłoszenia"))
                    //            {
                    //                return RedirectToAction("Index", "Task");
                    //            }
                    //            else if (roles.Contains("Klient"))
                    //            {
                    //                return RedirectToAction("Index", "Client");
                    //            }
                    //            else
                    //            {
                    //                return RedirectToAction("AccessDenied", "Error");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            return RedirectToAction("ChangePassword", "Account", new { user.UserId });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return RedirectToAction("ChangePassword", "Account", new { user.UserId});
                    //    }
                    //}
                    //else
                    //{
                    //    return RedirectToAction("AccountDenied", "Error");
                    //}
                }

                ModelState.AddModelError("", "Nieprawidłowy użytkownik lub hasło");
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {

                if (user.UserId == 0)
                {
                    user.Password = userUtils.PasswordHash(user.Password);
                    db.Users.Add(user);
                }
                else
                {
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

        public ViewResult YearView()
        {
            var model = from c in db.Set<Year>()
                        select c;
            return View(model);
        }
        public ViewResult YearAdd()
        {

            return View();
        }
        
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

        public ViewResult YearEdit(int YearId)
        {
            Year model = db.Set<Year>().ToList()
                .SingleOrDefault(p => p.YearId == YearId);
            return View(model);
        }

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
        public ViewResult GraduateAdd(int? YearId)
        {
            var model = new YearAndGraduate { Graduates = new Graduate { YearId = YearId } };
            return View(model);
        }
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
        public ViewResult GraduateEdit(int GraduateId)
        {
            
            Graduate model = db.Set<Graduate>().ToList()
                .SingleOrDefault(p => p.GraduateId == GraduateId);
            return View(model);
        }
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
