﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models.Models
{
    public class Ingredient : Entity
    {
        public Recipe Recipe { get; set; }

        public Product Product { get; set; }

        public string Metric { get; set; }

        public float Amount { get; set; }



    }
}
