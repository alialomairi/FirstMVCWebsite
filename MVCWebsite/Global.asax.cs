using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Entities;
using System.Web.Security;
using System.Security.Principal;

namespace MVCWebsite
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

            ViewEngineConfig.RegisterViewLocation(ViewEngines.Engines);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }

            if (authTicket == null) return;

            User user = null;

            int userId = int.Parse(authTicket.UserData);

            using (TeachMeContext  ctx = new TeachMeContext())
            {
                try
                {
                    user = ctx.Users.Find(userId);
                    if (user != null)
                    {
                        IIdentity identity = new UserIdentity(user.UserId, user.DisplayName,!string.IsNullOrWhiteSpace(user.FacebookId));
                        string role = user.UserType == UserType.Admin ? "Admin" : user.UserType == UserType.Teacher ? "Teacher" : "Student";
                        HttpContext.Current.User = new GenericPrincipal(identity, new string[]{role});
                    }
                }
                catch
                {
                }
            }
        }

    }
}