using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmartMeal.Models.Models;

namespace SmartMeal.Models.BindingModels
{
    public class TimetableBindingModel
    {
//        [JsonConverter(typeof(DateFormatConverter))]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MealDay {
            get { return _mealTime; }
            set { _mealTime = value.Date; } }

        [EnumDataType(typeof(MealTime), ErrorMessage = "Podana wartość dnia nie istnieje.")]
        public MealTime MealTime { get; set; }

        public long RecipeId { get; set; }

        private DateTime _mealTime { get; set; }
    }
}
