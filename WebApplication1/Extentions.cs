using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AllEngineers
{
    public static class Extentions
    {
        public static MvcHtmlString CategoryMenu(this HtmlHelper htmlHelper)
        {
            StringBuilder builder = new StringBuilder();
            using(TeachMeContext ctx = new TeachMeContext())
            {
                List<Category> parents = ctx.Categories.Where(x => x.ParentId == null).ToList();
                foreach(Category parent in parents)
                {
                    RenderCategory(parent, builder,"");
                }
            }
            return new MvcHtmlString(builder.ToString());
        }

        private static void RenderCategory(Category parent, StringBuilder builder, string path)
        {
            path += "/" +parent.CategoryKey;

            builder.Append("<li>");
            builder.AppendFormat("<a href=\"{0}\">{1}</a>", path, parent.CategoryName);
            if (parent.HasChilds || parent.HasTutorials)
            {
                builder.Append("<ul>");
                foreach(Category child in parent.Childs)
                {
                    RenderCategory(child, builder, path);
                }
                foreach(Material tutorial in parent.Tutorials)
                {
                    builder.AppendFormat("<li><a href=\"{0}/{1}\">{2}</a></li>",path, tutorial.MaterialKey, tutorial.MaterialName);
                }
                builder.Append("</ul>");
            }
            builder.Append("</li>");
        }

        public static void SendEmail(this Controller controller, string email, string title, string body)
        {
        }
    }
}