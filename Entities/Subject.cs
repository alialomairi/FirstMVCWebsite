using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string SubjectKey { get; set; }
        [NotMapped]
        public string Link
        {
            get
            {
                if (Parent == null)
                    return Tutorial.Link + "/" + SubjectKey;

                return Parent.Link + "/" + SubjectKey;
            }
        }
        [ForeignKey("Tutorial")]
        public int TutorialId { get; set; }
        public virtual Material Tutorial { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Subject Parent { get; set; }
        public virtual ICollection<Subject> Childs { get; set; }
        public string Content { get; set; }

    }
}