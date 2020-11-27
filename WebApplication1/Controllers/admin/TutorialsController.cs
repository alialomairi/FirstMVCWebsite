using Entities;
using AllEngineers.Models;
using AllEngineers.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Controllers.admin
{
    public class TutorialsController : Controller
    {
        private readonly TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }

        // GET: Tutorials
        public ActionResult Index(MaterialModel model)
        {
            ViewBag.TrainerList = ctx.Users.Where(x => x.UserType == UserType.Teacher && x.Enabled).Select(x => new SelectListItem { Text = x.DisplayName, Value = x.UserId.ToString() }).ToList();
            ViewBag.CategoryList = ctx.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString(),Group=new SelectListGroup { Name = x.ParentId.ToString() } }).ToList();

            IQueryable<Material> query = ctx.Materials.Where(x =>
            (model.CategoryId == null || x.CategoryId == model.CategoryId) &&
            (model.TrainerId == null || x.TrainerId == model.TrainerId) &&
            (model.TutorialName == null || x.MaterialName.Contains(model.TutorialName))
            ).OrderBy(x => x.MaterialId);

            ViewBag.totalcount = query.Count();
            ViewBag.pageSize = 10;
            ViewBag.Tutorials = query.Take(10).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult loadTutorials(MaterialModel model,int pageNumber, int pageSize)
        {
            PagingResponse response = new PagingResponse();

            IQueryable<Material> query = ctx.Materials.Where(x =>
            (model.CategoryId == null || x.CategoryId == model.CategoryId) &&
            (model.TrainerId == null || x.TrainerId == model.TrainerId) &&
            (model.TutorialName == null || x.MaterialName.Contains(model.TutorialName))
            ).OrderBy(x => x.MaterialId);

            response.totalCount = query.Count();
            List<Material> tutorials = query.Skip((pageNumber -1)*pageSize).Take(pageSize).ToList();

            response.html = this.RenderView("_TutorialList", tutorials);
            response.success = true;

            return Json(response);
        }
        public ActionResult addTutorial(MaterialModel model)
        {
            HtmlResponse response = new HtmlResponse();

            Material tutorial = new Material
            {
                MaterialName = model.TutorialName,
                TrainerId = model.TrainerId.Value
            };
            ctx.Materials.Add(tutorial);

            ctx.SaveChanges();

            response.html = this.RenderView("_TutorialList", new List<Material> { tutorial });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult getTutorial(int id)
        {
            DataResponse response = new DataResponse();

            Material tutorial = ctx.Materials.Find(id);
            response.success = true;

            response.data = new MaterialModel
            {
                TutorialName = tutorial.MaterialName,
                TrainerId = tutorial.TrainerId
            };


            return Json(response);
        }
        [HttpPost]
        public ActionResult editTutorial(int id, MaterialModel model)
        {
            HtmlResponse response = new HtmlResponse();

            Material tutorial = ctx.Materials.Find(id);

            tutorial.MaterialName = model.TutorialName;
            tutorial.TrainerId = model.TrainerId.Value;

            ctx.SaveChanges();

            response.html = this.RenderView("_TutorialList", new List<Material> { tutorial });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult deleteTutorial(int id)
        {
            JsonResponse response = new JsonResponse();

            Material tutorial = ctx.Materials.Find(id);

            ctx.Materials.Remove(tutorial);
            ctx.SaveChanges();

            response.success = true;

            return Json(response);
        }
    }
}