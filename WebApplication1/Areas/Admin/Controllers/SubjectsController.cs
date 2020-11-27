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
    public class SubjectsController : Controller
    {
        private readonly TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }

        // GET: Admin/Subjects
        public ActionResult Index(SubjectModel model)
        {
            Material tutorial = ctx.Materials.Find(model.TutorialId.Value);
            Category category = tutorial.Category;

            ViewBag.TutorialId = model.TutorialId;
            ViewBag.CategoryList = ctx.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString(), Group = new SelectListGroup { Name = x.ParentId.ToString() } }).ToList();
            ViewBag.TutorialList = category.Tutorials.Select(x => new SelectListItem { Text = x.MaterialName, Value = x.MaterialId.ToString() }).ToList();
            ViewBag.SubjectList = tutorial.Subjects.Select(x => new SelectListItem { Text = x.Title, Value = x.SubjectId.ToString(), Group = new SelectListGroup { Name = x.ParentId.ToString() } }).ToList();

            IQueryable<Subject> query = ctx.Subjects.Where(x =>
            (model.TutorialId == null || x.TutorialId == model.TutorialId) &&
            (x.ParentId == model.ParentId) &&
            (model.Title == null || x.Title.Contains(model.Title))
            ).OrderBy(x => x.SubjectId);

            ViewBag.totalcount = query.Count();
            ViewBag.pageSize = 10;
            ViewBag.Subjects = query.Take(10).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult loadSubjects(SubjectModel model, int pageNumber, int pageSize)
        {
            PagingResponse response = new PagingResponse();

            IQueryable<Subject> query = ctx.Subjects.Where(x =>
            (model.TutorialId == null || x.TutorialId == model.TutorialId) &&
            (x.ParentId == model.ParentId) &&
            (model.Title == null || x.Title.Contains(model.Title))
            ).OrderBy(x => x.SubjectId);

            response.totalCount = query.Count();
            List<Subject> tutorials = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            response.html = this.RenderView("_SubjectList", tutorials);
            response.success = true;

            return Json(response);
        }
        public ActionResult addSubject(int? TutorialId, int? id)
        {
            if (id != null)
            {
                Subject subject = ctx.Subjects.Find(id.Value);
                ViewData.Model = new SubjectModel
                {
                    Title = subject.Title,
                    SubjectKey = subject.SubjectKey,
                    ParentId = subject.ParentId,
                    Content = subject.Content,
                    TutorialId = subject.TutorialId
                };
                TutorialId = subject.TutorialId;
            }
            Material tutorial = ctx.Materials.Find(TutorialId.Value);
            ViewBag.TutorialId = TutorialId.Value;
            ViewBag.SubjectList = tutorial.Subjects.Select(x => new SelectListItem { Text = x.Title, Value = x.SubjectId.ToString(), Group = new SelectListGroup { Name = x.ParentId.ToString() } }).ToList();

            return View("_SubjectDialog");
        }
        [HttpPost]
        public ActionResult addSubject(SubjectModel model, int? id)
        {
            if (id == null)
            {
                Subject subject = new Subject
                {
                    TutorialId = model.TutorialId.Value,
                    ParentId = model.ParentId,
                    Title = model.Title,
                    SubjectKey = model.SubjectKey,
                    Content = model.Content
                };
                ctx.Subjects.Add(subject);
            }
            else
            {
                Subject subject = ctx.Subjects.Find(id.Value);

                model.TutorialId = subject.TutorialId;
                subject.ParentId = model.ParentId;
                subject.Title = model.Title;
                subject.SubjectKey = model.SubjectKey;
                subject.Content = model.Content;
            }

            ctx.SaveChanges();

            return Redirect("~/admin/Subjects?TutorialId=" + model.TutorialId.ToString());
        }
        [HttpPost]
        public ActionResult deleteTutorial(int id)
        {
            JsonResponse response = new JsonResponse();

            Subject subject = ctx.Subjects.Find(id);

            ctx.Subjects.Remove(subject);
            ctx.SaveChanges();

            response.success = true;

            return Json(response);
        }
    }
}