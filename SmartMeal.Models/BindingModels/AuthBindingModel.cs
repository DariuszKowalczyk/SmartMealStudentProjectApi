using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SmartMeal.Service;

namespace SmartMeal.Models.BindingModels
{
    public class AuthBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password
        {
            get { return _password; }
            set { _password = HashManager.GetHash(value); }
        }

        private string _password;
    }
}
