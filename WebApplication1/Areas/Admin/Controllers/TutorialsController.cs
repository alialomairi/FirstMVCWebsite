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
            ViewBag.CategoryList = ctx.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString(), Group = new SelectListGroup { Name = x.ParentId.ToString() } }).ToList();

            IQueryable<Material> query = ctx.Materials.Where(x =>
            (model.CategoryId == null || x.CategoryId == model.CategoryId) &&
            (model.TrainerId == null || x.TrainerId == model.TrainerId) &&
            (model.TutorialName == null || x.MaterialName.Contains(model.TutorialName))
            ).OrderBy(x => x.MaterialId);

            ViewBag.CategoryId = model.CategoryId;
            ViewBag.totalcount = query.Count();
            ViewBag.pageSize = 10;
            ViewBag.Tutorials = query.Take(10).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult loadTutorials(MaterialModel model, int pageNumber, int pageSize)
        {
            PagingResponse response = new PagingResponse();

            IQueryable<Material> query = ctx.Materials.Where(x =>
            (model.CategoryId == null || x.CategoryId == model.CategoryId) &&
            (model.TrainerId == null || x.TrainerId == model.TrainerId) &&
            (model.TutorialName == null || x.MaterialName.Contains(model.TutorialName))
            ).OrderBy(x => x.MaterialId);

            response.totalCount = query.Count();
            List<Material> tutorials = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            response.html = this.RenderView("_TutorialList", tutorials);
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult addTutorial(MaterialModel model, int? id)
        {
            if (id == null)
            {

                Material tutorial = new Material
                {
                    MaterialName = model.TutorialName,
                    MaterialKey = model.TutorialKey,
                    CategoryId = model.CategoryId,
                    TrainerId = model.TrainerId.Value,
                    Description = model.Description
                };
                ctx.Materials.Add(tutorial);
            }
            else
            {
                Material tutorial = ctx.Materials.Find(id.Value);

                tutorial.MaterialName = model.TutorialName;
                tutorial.MaterialKey = model.TutorialKey;
                tutorial.Description = model.Description;
                tutorial.CategoryId = model.CategoryId;
                tutorial.TrainerId = model.TrainerId.Value;
            }

            ctx.SaveChanges();

            return Redirect("~/admin/tutorials?CategoryId=" + model.CategoryId.ToString());
        }
        public ActionResult addTutorial(int? id,int? CategoryId)
        {
            ViewBag.TrainerList = ctx.Users.Where(x => x.UserType == UserType.Teacher && x.Enabled).Select(x => new SelectListItem { Text = x.DisplayName, Value = x.UserId.ToString() }).ToList();
            ViewBag.CategoryList = ctx.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString(), Group = new SelectListGroup { Name = x.ParentId.ToString() } }).ToList();

            MaterialModel model = new MaterialModel();
            ViewData.Model = model;

            if (id == null)
            {
                model.CategoryId = CategoryId;
            }
            else
            {
                Material tutorial = ctx.Materials.Find(id);

                model.TutorialName = tutorial.MaterialName;
                model.TutorialKey = tutorial.MaterialKey;
                model.CategoryId = tutorial.CategoryId;
                model.Description = tutorial.Description;
                model.TrainerId = tutorial.TrainerId;
            }

            return View("_TutorialDialog");
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