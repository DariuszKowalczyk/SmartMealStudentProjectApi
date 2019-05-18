using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;

namespace SmartMeal.Models
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter()
        {
            base.DateTimeFormat = "dd-MM-yyyy";
        }
    }
}
