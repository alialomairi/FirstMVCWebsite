using AllEngineers.Models;
using AllEngineers.Models.Response;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers.Controllers.admin
{
    public class CategoriesController : Controller
    {
        private readonly TeachMeContext ctx = new TeachMeContext();
        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
            base.Dispose(disposing);
        }
        // GET: Categories
        public ActionResult Index(CategoryModel model)
        {
            ViewBag.CategoryList = ctx.Categories.Select(x => new SelectListItem
            {
                Group = new SelectListGroup { Name=x.ParentId.ToString()},
                Value = x.CategoryId.ToString(),
                Text= x.CategoryName
            }).ToList();

            IQueryable<Category> query = ctx.Categories.Where(x => x.ParentId == model.ParentId).OrderBy(x => x.CategoryId);
            ViewBag.totalcount = query.Count();

            ViewBag.Categories = query.Take(10).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult reloadCategories(CategoryModel model, int pageNumber, int pageSize)
        {
            PagingResponse response = new PagingResponse();

            IQueryable<Category> query = ctx.Categories.Where(x => x.ParentId == model.ParentId).OrderBy(x => x.CategoryId);
            response.totalCount = query.Count();

            List<Category> categories = query.Take(10).ToList();

            response.html = this.RenderView("_CategoryList", categories);
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult addCategory(CategoryModel model)
        {
            HtmlResponse response = new HtmlResponse();

            Category category = new Category
            {
                CategoryName = model.CategoryName,
                CategoryKey = model.CategoryKey,
                ParentId = model.ParentId
            };
            ctx.Categories.Add(category);

            ctx.SaveChanges();

            response.html = this.RenderView("_CategoryList", new List<Category> { category });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult getCategory(int id)
        {
            DataResponse response = new DataResponse();

            Category category = ctx.Categories.Find(id);
            response.success = true;

            response.data = new CategoryModel
            {
                CategoryKey = category.CategoryKey,
                CategoryName = category.CategoryName,
                ParentId = category.ParentId
            };


            return Json(response);
        }
        [HttpPost]
        public ActionResult editCategory(int id, CategoryModel model)
        {
            HtmlResponse response = new HtmlResponse();

            Category category = ctx.Categories.Find(id);

            category.CategoryName = model.CategoryName;
            category.CategoryKey = model.CategoryKey;
            category.ParentId = model.ParentId;

            ctx.SaveChanges();

            response.html = this.RenderView("_CategoryList", new List<Category> { category });
            response.success = true;

            return Json(response);
        }
        [HttpPost]
        public ActionResult deleteCategory(int id)
        {
            JsonResponse response = new JsonResponse();

            Category category = ctx.Categories.Find(id);

            ctx.Categories.Remove(category);
            ctx.SaveChanges();

            response.success = true;

            return Json(response);
        }
    }
}