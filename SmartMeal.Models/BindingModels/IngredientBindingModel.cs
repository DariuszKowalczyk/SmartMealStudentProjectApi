using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models.BindingModels
{
    public class IngredientBindingModel
    {
        public long ProductId { get; set; }

        public float Amount { get; set; }

        public string Metric { get; set; }
    }
}
