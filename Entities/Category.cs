using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryKey { get; set; }
        [NotMapped]
        public string Link { 
            get
            {
                if (ParentId == null)
                    return "/" + CategoryKey;
                return Parent.Link + "/" + CategoryKey;
            }
        }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Material> Tutorials { get; set; }
        public virtual ICollection<Category> Childs { get; set; }
        [NotMapped]
        public bool HasChilds { get { return Childs.Count > 0; } }
        [NotMapped]
        public bool HasTutorials { get { return Tutorials.Count > 0; } }
    }
}