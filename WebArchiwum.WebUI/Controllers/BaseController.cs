using Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebArchiwum.WebUI.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

    }
}
