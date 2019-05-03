using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Service;

namespace SmartMeal.Models.ModelsDto
{
    public class LoginDto
    {
        public string Email { get; set; }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = HashManager.GetHash(value); }
        }
    }
}
