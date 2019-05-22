using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Models.Models;

namespace SmartMeal.Models.ModelsDto
{
    public class IngredientDto : DtoBaseModel
    {
        public int Id { get; set; }

        public ProductDto Product { get; set; }

        public float Amount { get; set; }

        public Metrics Metric { get; set; }

    }
}
