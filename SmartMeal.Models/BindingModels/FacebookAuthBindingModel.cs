using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMeal.Models.BindingModels
{
    public class FacebookAuthBindingModel
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
