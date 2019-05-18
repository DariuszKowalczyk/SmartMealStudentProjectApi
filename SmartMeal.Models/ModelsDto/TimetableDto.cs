using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Models.Models;

namespace SmartMeal.Models.ModelsDto
{
    public class TimetableDto : DtoBaseModel
    {
        public long Id { get; set; }

        public long RecipeId { get; set; }

        public DateTime MealDay { get; set; }

        public MealTime MealTime { get; set; }

    }
}
