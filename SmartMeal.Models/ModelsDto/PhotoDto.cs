using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models.ModelsDto
{
    public class PhotoDto : DtoBaseModel
    {
        public long Id { get; set; }
        public string ImagePath { get; set; }
    }
}
