using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Models.BindingModels
{
    public class RecipeBindingModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public List<IngredientBindingModel> Ingredients { get; set; } = new List<IngredientBindingModel>();
    }
}
