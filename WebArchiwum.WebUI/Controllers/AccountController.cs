using Domain.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebArchiwum.WebUI.Models;
using Domain.Utils;

namespace WebArchiwum.WebUI.Controllers
{
    public class AccountController : Controller
    {
        MyDbContext db = new MyDbContext();
        UserUtils userUtils = new UserUtils();
        //
        // GET: /Account/Register

        public ViewResult Login()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
           
                if (user.UserId == 0)
                {
                    user.Password = userUtils.PasswordHash(user.Password);
                    db.Users.Add(user);
                }
                else
                {
                    User dbEntry = db.Users.Find(user.UserId);
                    if(dbEntry!=null)
                    {
                        dbEntry.Login = user.Login;
                        dbEntry.Password = user.Password; 
                    }
                }
                db.SaveChanges();
                TempData["message"] = string.Format("Zapisano {0} ", user.Login);
                return RedirectToAction("Index");
            }
            else
            {
                return View(user);
            }
            
        }

      
    }
   
}

 