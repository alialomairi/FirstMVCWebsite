using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Node
    {
        public int NodeId { get; set; }
        [MaxLength(50)]
        public string Url { get; set; }
        [MaxLength(50)]
        public string RewritePath { get; set; }
   }
}