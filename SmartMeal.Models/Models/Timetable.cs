using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace SmartMeal.Models.Models
{
    public class Timetable : Entity
    {
        public Recipe Recipe { get; set; }

        public MealTime MealTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MealDay { get; set; }
    }

    public enum MealTime : int
    {
        [Description("śniadanie")]
        Breakfast = 1,

        [Description("drugie śniadanie")]
        Lunch = 2,

        [Description("obiad")]
        Dinner = 3,

        [Description("podwieczorek")]
        Timetea = 4,

        [Description("kolacja")]
        Supper = 5


    }
}
