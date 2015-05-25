using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebArchiwum.WebUI.Controllers
{
    public class AjaxAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {            
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                throw new Exception("Obsługa JavaScript musi być włączona.");
            }            
        }
    }
}
