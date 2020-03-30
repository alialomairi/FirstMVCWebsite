using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Material
    {
        public int MaterialId { get; set; }
        [MaxLength(50)]
        public string MaterialName { get; set; }
    }
}
