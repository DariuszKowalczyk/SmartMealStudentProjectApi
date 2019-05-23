using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Models
{
    public static class Error
    {
        // Product
        public const string ProductExist = "Produkt o danej nazwie już istnieje.";
        public const string ProductDoesntExist = "Nie odnaleziono danego produktu.";
        public const string ProductErrorWhenDelete = "Wystąpił błąd podczas usuwania produktu.";
        public const string ProductErrorWhenUpdate = "Wystąpił błąd podczas aktualizacji produktu";

        public const string IngredientCreateFails = "Błąd podczas tworzenia składników";
        public const string AuthenticationError = "Bad login and/or password.";
        public const string UserExist = "The user already exists.";
        public const string RegisterDtoIsNotValid = "Data is not valid.";
        public const string RecipeExist = "The recipe already exists.";
        public const string RecipeDoesntExist = "The recipe doesn't exist";
        public const string TimetableDoesntExist = "The timmetable doesn't exist";
        public const string FileIsEmpty = "Przesłano pusty plik.";
        public const string FileWrongContentType = "Nieobsługiwany format danych.";
        public const string PhotoDoesntExist = "The photo doesnt exist.";
        
    }
}
