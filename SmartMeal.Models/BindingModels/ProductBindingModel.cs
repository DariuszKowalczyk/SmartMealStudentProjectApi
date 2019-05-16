using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMeal.Models.BindingModels
{
    public class ProductBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImagePath { get; set; }
    }
}
