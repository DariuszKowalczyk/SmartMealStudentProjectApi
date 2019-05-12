using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models.Models
{
    public class Recipe : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

    }
}
