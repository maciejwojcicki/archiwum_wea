using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebArchiwum.WebUI.Interface;
using WebArchiwum.WebUI.Service;

namespace WebArchiwum.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        //{
        //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {

        //        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        //        CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
        //        CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
        //        newUser.UserId = serializeModel.UserId;
        //        newUser.Login = serializeModel.FirstName;
        //        newUser.Password = serializeModel.LastName;
        //        newUser.roles = serializeModel.roles;

        //        HttpContext.Current.User = newUser;
        //    }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    int userId = int.Parse(authTicket.Name);
                    IUserService userService = new UserService();
                    try
                    {
                        HttpContext.Current.User = userService.GetPrincipal(userId);
                        Response.Cookies.Add(authCookie);
                    }
                    catch (Exception ex)
                    {
                        FormsAuthentication.SignOut();
                        Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                        HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(""), null);
                    }
                }
            }
        }

        
    }
}