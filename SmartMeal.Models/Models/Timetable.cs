using System;
using System.Buffers;
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

        public User Owner { get; set; }
    }

    public enum MealTime : int
    {
        [Description("śniadanie")]
        Breakfast = 0,

        [Description("drugie śniadanie")]
        Lunch = 1,

        [Description("obiad")]
        Dinner = 2,

        [Description("podwieczorek")]
        Timetea = 3,

        [Description("kolacja")]
        Supper = 4


    }
}
