using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ContentPage
    {
        [Key]
        public int PageId { get; set; }
        [MaxLength(50)]
        public string Url { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string PageContent { get; set; }

    }
}