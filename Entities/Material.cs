using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Material
    {
        public int MaterialId { get; set; }
        [MaxLength(50)]
        public string MaterialName { get; set; }

        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public virtual User Trainer { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
        public string MaterialKey { get; set; }
    }
}
