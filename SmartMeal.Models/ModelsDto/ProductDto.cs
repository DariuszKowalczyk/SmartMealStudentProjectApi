using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMeal.Models.ModelsDto
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int? Id { get; set; }

        public string ImagePath { get; set; }
    }
}
