using Domain.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebArchiwum.WebUI.Models;

namespace WebArchiwum.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        MyDbContext db = new MyDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Ajax]
        public ActionResult AppModel()
        {
            var model = from c in db.Set<Year>()
                        orderby c.Name
                        select c;

            var data = new
            {
                Years = model.Select(m => new
                {
                    Name = m.Name,
                    YearId = m.YearId
                })
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Graduates(int YearId)
        {
            var model = from o in db.Set<Year>()
                        join o2 in db.Set<Graduate>()
                        on o.YearId equals o2.YearId
                        where o.YearId.Equals(YearId)
                        select new YearAndGraduate { Years = o, Graduates = o2 };

            var data = new
            {
                graduates = model.Select(m=> new
                {
                    FirstName = m.Graduates.FirstName,
                    LastName = m.Graduates.LastName
                })
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
