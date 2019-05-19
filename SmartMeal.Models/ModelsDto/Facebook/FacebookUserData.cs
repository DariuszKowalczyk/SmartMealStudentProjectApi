using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SmartMeal.Models.ModelsDto
{
    public class FacebookUserData
    {
        public long Id
        {
            get { return FacebookId;}
            set { FacebookId = value; }
        }
        public long FacebookId { get; set; }
        public string Email { get; set; }
    }




}
