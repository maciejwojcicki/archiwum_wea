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
                             user.Email,
                             DateTime.Now,
                             DateTime.Now.AddDays(1),
                             false,
                             userData);
                  
                    //Ustawia ciasteczko
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    return RedirectToAction("Index", "Test");
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


    }
}
