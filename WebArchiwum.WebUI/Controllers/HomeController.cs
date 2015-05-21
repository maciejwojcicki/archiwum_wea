using Domain.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebArchiwum.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        MyDbContext db = new MyDbContext();

        public ActionResult Index()
        {
            return View();
        }
        public ViewResult GraduateView()
        {
            var model = from c in db.Set<Graduate>()
                        select c;
                        
            return View(model);
        }
        public ViewResult GraduateAdd()
        {
            
            return View();
        } 
        [HttpPost]
        public ActionResult GraduateAdd(Graduate graduate)
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
                TempData["message"] = string.Format("Zapisano {0} ", graduate.FirstName+graduate.LastName);
                return RedirectToAction("Index");
            }
            else
            {
                return View(graduate);
            }
        }

    }
}
