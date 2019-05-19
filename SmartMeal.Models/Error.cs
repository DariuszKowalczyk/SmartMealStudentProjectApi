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
        public const string RecipeExist = "The recipe already exists.";
        public const string RecipeDoesntExist = "The recipe doesn't exist";
        public const string TimetableDoesntExist = "The timmetable doesn't exist";
        public const string FileIsEmpty = "Przesłano pusty plik.";
        public const string FileWrongContentType = "Nieobsługiwany format danych.";
    }
}
