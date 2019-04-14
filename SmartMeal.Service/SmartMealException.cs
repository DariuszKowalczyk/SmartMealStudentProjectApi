using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMeal.Service
{
    public class SmartMealException : Exception
    {
        public List<string> errors { get; set; }

        public SmartMealException(params string[] errors) : base()
        {
            this.errors = errors.ToList<string>();
        }
    }
}
