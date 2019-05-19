using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models.Models
{
    public class Photo : Entity
    {
        public string ContentType { get; set; }

        public string Filename { get; set; }

        public long Size { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
