using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace SmartMeal.Models.Models
{
    public class Ingredient : Entity
    {
        public Recipe Recipe { get; set; }

        public Product Product { get; set; }

        public Metrics Metric { get; set; }

        public float Amount { get; set; }

    }

    public enum Metrics : int
    {
        [Description("Łyżki")]
        Spoon = 0,

        [Description("Łyżeczki")]
        Tsp = 1,

        [Description("")]
        Pieces = 2,

        [Description("szczypta")]
        Pinch = 3,

        [Description("gr")]
        Gram = 4,

        [Description("kg")]
        Kilogram = 5,

        [Description("l")]
        Liter = 6,

        [Description("ml")]
        MiliLiter = 7,

        [Description("szklanki")]
        Glass = 8
    }
}
