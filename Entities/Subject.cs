using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        [ForeignKey("Tutorial")]
        public int TutorialId { get; set; }
        public virtual Material Tutorial { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Subject Parent { get; set; }
        public string Content { get; set; }

    }
}