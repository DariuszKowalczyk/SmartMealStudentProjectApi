using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SmartMeal.Models.Models;

namespace SmartMeal.Models.BindingModels
{
    public class IngredientBindingModel
    {
        public long ProductId { get; set; }

        public float Amount { get; set; }

        [EnumDataType(typeof(Metrics), ErrorMessage = "Podana miara nie istnieje.")]
        public string Metric { get; set; }
    }
}
