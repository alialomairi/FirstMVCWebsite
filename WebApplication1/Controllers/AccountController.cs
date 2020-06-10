using Entities;
using AllEngineers.Models;
using AllEngineers.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllEngineers.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return MyProfile();
        }
        [HttpPost]
        public ActionResult Index(UserModel model)
        {
            return MyProfile(model);
        }

        public ActionResult SignIn()
        {
            ViewBag.Invalid = true;
            ViewBag.Title = "Sign In";
            return View();
        }
        private TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult addMaterial(string text)
        {
            DataResponse response = new DataResponse();
            Material material = new Material { MaterialName = text };

            try
            {
                ctx.Materials.Add(material);
                ctx.SaveChanges();

                response.data = new
                {
                    text = text,
                    value = material.MaterialId
                };
                response.success = true;
            }
            catch (Exception ex)
            {
                response.error = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult facebooklogin(FacebookLoginModel model)
        {
            RedirectResponse response = new RedirectResponse();

            try
            {
                    User user = ctx.Users.SingleOrDefault(x => x.FacebookId == model.id);

                    if (user == null)
                    {
                        user = new User
                     {
                         FacebookId = model.id,
                         FullName = model.name,
                         DisplayName = model.first_name,
                         Email = model.email
                     };

                        ctx.Users.Add(user);

                        ctx.SaveChanges();
                    }
                    LoginUser(user);

                    response.success = true;
                    response.redirectUrl = Url.Content("~/");
                }
            catch (Exception ex)
            {
                response.error = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel model)
        {
            ViewBag.Invalid = false;

            if (ModelState.IsValid)
            {
                    User user = ctx.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password&&x.Enabled);

                    if (user != null)
                    {
                        DateTime issueDate = DateTime.Now;
                        DateTime expiration = model.RememberMe? issueDate.AddDays(30) : issueDate + FormsAuthentication.Timeout;

                        //Create the ticket, and add the groups.
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                                  user.FullName, issueDate, expiration, true, user == null ? "1" : user.UserId.ToString());

                        //Encrypt the ticket.
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        //Create a cookie, and then add the encrypted ticket to the cookie as data.
                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        authCookie.Expires = authTicket.Expiration;

                        //Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);

                        string returnurl = Server.UrlDecode(Request.QueryString["url"]);

                        Response.Redirect(returnurl ?? (user.UserType == UserType.Admin?"~/admin": "~/"));
                    }
                    ViewBag.Invalid = true;
                }
            
            ViewBag.Title = "Sign In";
            return View();
        }

        private void LoginUser(User user, bool rememberme = false)
        {
            DateTime issueDate = DateTime.Now;
            DateTime expiration = rememberme ? issueDate.AddDays(30) : issueDate + FormsAuthentication.Timeout;

            //Create the ticket, and add the groups.
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                      user.FullName, issueDate, expiration, true, user == null ? "1" : user.UserId.ToString());

            //Encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            //Create a cookie, and then add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Expires = authTicket.Expiration;

            //Add the cookie to the outgoing cookies collection.
            Response.Cookies.Add(authCookie);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            Response.Redirect("~/");

            return View("Index");
        }

        public ActionResult SignUp()
        {
            ViewBag.Title = "Create User";
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FullName = model.FullName,
                    DisplayName = model.DisplayName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Username = model.Username,
                    Password = model.Password
                };
                ctx.Users.Add(user);

                ctx.SaveChanges();

                string html = this.RenderView("ActivationEmail", model);
                this.SendEmail(model.Email, "Account Activation", html);

                Response.Redirect("~/");
            }

            ViewBag.Title = "Create User";
            return View();
        }

        public ActionResult Activate(string id)
        {
            return View();
        }

        public ActionResult Reset(int? id)
        {
            if (id.HasValue)
            {
                User user = ctx.Users.Find(id);
                ViewData.Model = new ResetAccountModel { Email = user.Email };
            }

            return View();
        }

        [HttpPost]
        public ActionResult Reset(ResetAccountModel model)
        {
            if (ModelState.IsValid)
            {
                User user = ctx.Users.SingleOrDefault(x => x.Email == model.Email);

                if (user != null)
                {
                    user.Password = model.Password;
                    user.Enabled = false;

                    string html = this.RenderView("ResetEmail", model);
                    this.SendEmail(model.Email, "Resert Password", html);

                    Response.Redirect("~/");
                }
            }
            
            return View();
        }

        public ActionResult MyProfile()
        {
            if (User == null || !User.Identity.IsAuthenticated)
                Response.Redirect("~/");

            int userid = int.Parse((User.Identity as FormsIdentity).Ticket.UserData);
            User user = ctx.Users.Find(userid);

            UserModel model = new UserModel
            {
                FullName = user.FullName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username
            };

            ViewData.Model = model;
            ViewBag.UserId = user.UserId;

            SelectListItem[] materials = ctx.Materials.Select<Material, SelectListItem>(x => new SelectListItem { 
                Text=x.MaterialName,
                Value=x.MaterialId.ToString()
            }).ToArray();
            ViewBag.Materials = materials;

            ViewBag.Title = "My Profile";
            return View("MyProfile");
        }


        [HttpPost]
        public ActionResult MyProfile(UserModel model)
        {
            if (User == null || !User.Identity.IsAuthenticated)
                Response.Redirect("~/");

            if (ModelState.IsValid)
            {
                int userid = int.Parse((User.Identity as FormsIdentity).Ticket.UserData);
                User user = ctx.Users.Find(userid);

                user.FullName = model.FullName;
                user.Username = model.Username;
                user.DisplayName = model.DisplayName;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Password = string.IsNullOrWhiteSpace(model.Password) ? user.Password : model.Password;

                ctx.SaveChanges();

                Response.Redirect("~/");
            }

            return MyProfile();
        }

        public void SavePicture(int id,int width, int height, float zoom, float left, float top)
        {
            HttpPostedFileBase picture = Request.Files["picture"];

            System.Drawing.Image original = System.Drawing.Image.FromStream(picture.InputStream);
            int owidth = original.Width;
            int oheight = original.Height;

            int swidth = (int)(owidth * zoom);
            int sheight = (int)(oheight * zoom);

            System.Drawing.Bitmap pic = new System.Drawing.Bitmap(width, height);

            using (var graphics = System.Drawing.Graphics.FromImage(pic))
                graphics.DrawImage(original, left, top, swidth, sheight);


            pic.Save(Server.MapPath("~/pictures/") + id.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

    }
}
