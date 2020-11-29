using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace AllEngineers
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity identity)
            {
                FormsAuthenticationTicket authTicket = identity.Ticket;
                if (int.TryParse(authTicket.UserData, out int userid))
                {
                    using (TeachMeContext ctx = new TeachMeContext())
                    {
                        User user = ctx.Users.Find(userid);
                        string[] roles = { "Student" };
                        if (user.UserType == UserType.Admin) roles[0] = "Admin";
                        else if (user.UserType == UserType.Teacher) roles[0] = "Teacher";

                        HttpContext.Current.User = new GenericPrincipal(identity, roles);
                    }
                }
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string url = Request.Url.AbsolutePath;
            string search = Request.QueryString.ToString();

            string newPath = url;
            string info = "";

            string[] parts = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                string part1 = parts[0];

                if (part1 != "admin" && part1 != "trainer")
                {
                    using (TeachMeContext ctx = new TeachMeContext())
                    {
                        ContentPage page = null;
                        Node node = ctx.Nodes.SingleOrDefault(x => x.Url == url);
                        if (node != null)
                            newPath = node.RewritePath;
                        else if ((page = ctx.ContentPages.SingleOrDefault(x => x.Url == url)) != null)
                            newPath = "~/content/index/" + page.PageId;
                        else
                        {
                            Category parent = ctx.Categories.SingleOrDefault(x => x.CategoryKey == part1 && x.ParentId == null);

                            if (parent == null)
                                return;

                            if (parts.Length == 1)
                                newPath = "~/home/category/" + parent.CategoryId.ToString();
                            else
                            {
                                int i = 1;
                                string part = parts[i];

                                // category
                                while (i < parts.Length)
                                {
                                    part = parts[i];

                                    Category category = parent.Childs.SingleOrDefault(x => x.CategoryKey == part);
                                    if (category == null)
                                        break;

                                    parent = category;
                                    i++;
                                }

                                // tutorial
                                if( i < parts.Length)
                                {
                                    part = parts[i];
                                    Material tutorial = parent.Tutorials.SingleOrDefault(x => x.MaterialKey == part);
                                    i++;

                                    Subject subject = null;
                                    bool video = false;
                                    while ( i < parts.Length) {
                                        part = parts[i];

                                        video = (part == "video");
                                        if (video)
                                            break;

                                        Subject sub = tutorial.Subjects.SingleOrDefault(x => x.SubjectKey == part);
                                        if (sub == null)
                                            break;

                                        subject = sub;
                                        i++;
                                    }

                                    if (subject != null)
                                        newPath = "~/home/subject/" + subject.SubjectId.ToString();
                                    else
                                    {
                                        newPath = "~/home/tutorial/" + tutorial.MaterialId.ToString();
                                        search = "v=" + video.ToString();
                                    }
                                }
                                else
                                    newPath = "~/home/category/" + parent.CategoryId.ToString();
                            }
                        }
                    }
                }
                else
                    return;
            }
            HttpContext.Current.RewritePath(newPath, info, search);

        }
    }
}
