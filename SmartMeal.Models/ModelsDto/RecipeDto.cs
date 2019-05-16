using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Models.Models;

namespace SmartMeal.Models.ModelsDto
{
    public class RecipeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string ImagePath { get; set; }

//        public IEnumerable<IngredientDto> Ingredients { get; set; }

    }
}
