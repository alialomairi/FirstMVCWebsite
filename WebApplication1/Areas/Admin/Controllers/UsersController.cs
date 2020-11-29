using AllEngineers.Models;
using AllEngineers.Models.Response;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        //
        // GET: /Users/

        public ActionResult Index()
        {
            ViewBag.Users = ctx.Users.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult addUser(UserModel model)
        {
            HtmlResponse response = new HtmlResponse();

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

            response.html = this.RenderView("_UserList", new List<User> { user });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult getUser(int id)
        {
            DataResponse response = new DataResponse();

            User user = ctx.Users.Find(id);
            response.success = true;

            response.data = new UserModel
            {
                FullName = user.FullName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username
            };


            return Json(response);
        }
        [HttpPost]
        public ActionResult editUser(int id, UserModel model)
        {
            HtmlResponse response = new HtmlResponse();

            User user = ctx.Users.Find(id);

            user.FullName = model.FullName;
            user.DisplayName = model.DisplayName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Username = model.Username;
            user.Password = string.IsNullOrEmpty(model.Password) ? user.Password : model.Password;

            ctx.SaveChanges();

            response.html = this.RenderView("_UserList", new List<User> { user });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult toggleEnable(int id)
        {
            HtmlResponse response = new HtmlResponse();

            User user = ctx.Users.Find(id);
            user.Enabled = !user.Enabled;

            ctx.SaveChanges();

            response.html = this.RenderView("_UserList", new List<User> { user });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult deleteUser(int id)
        {
            JsonResponse response = new JsonResponse();

            User user = ctx.Users.Find(id);

            ctx.Users.Remove(user);
            ctx.SaveChanges();

            response.success = true;

            return Json(response);
        }

    }
}