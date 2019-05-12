using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models
{
    public static class Error
    {
        public const string UserExist = "The user already exists.";
        public const string RegisterDtoIsNotValid = "Data is not valid.";
        public const string ProductExist = "The product already exists.";
        public const string ProductDoesntExist = "The product doesn't exist.";
    }
}
