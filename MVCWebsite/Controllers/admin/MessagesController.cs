using Entities;
using MVCWebsite.Models;
using MVCWebsite.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebsite.Controllers.admin
{
    public class MessagesController : Controller
    {
        private TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        //
        // GET: /Messages/

        public ActionResult Index()
        {
            ViewBag.Messages = ctx.Messages.Where(x=>x.ParentId==null).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult AddMessage(MessageModel model)
        {
            HtmlResponse response = new HtmlResponse();

            Message message = new Message
            {
                MessageSubject = model.Subject,
                MessageText = model.Text,
                MessageType = (MessageType) model.Type,
                ParentId = model.ParentId
            };

            ctx.Messages.Add(message);
            ctx.SaveChanges();

            response.success = true;
            response.html = this.RenderView("_MessageList", new List<Message> { message });

            return Json(response);
        }

    }
}
