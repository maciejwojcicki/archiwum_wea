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

    }
}
