using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebsite
{
    public class AdminAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get { return "admin"; }
        }

        public override void RegisterArea(AreaRegistrationContext routes)
        {
            routes.MapRoute(
                name: "admin",
                url: "admin/{controller}/{action}/{id}",
                defaults: new { area = "admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MVCWebsite.Controllers.admin" }
            );
        }
    }
}