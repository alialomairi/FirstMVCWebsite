using System.Web.Mvc;

namespace AllEngineers.Models
{
    public class SubjectModel
    {
        public string Title { get; set; }
        public string SubjectKey { get; set; }
        public int? TutorialId { get; set; }
        public int? ParentId { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}