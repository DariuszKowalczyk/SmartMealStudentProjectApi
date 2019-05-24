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

        // Recipe
        public const string RecipeExist = "Przepis o danej nazwie już istnieje.";
        public const string RecipeDoesntExist = "Nie odnaleziono danego przepisu.";
        public const string RecipeErrorWhenCreated = "Wystąpił błąd poczas tworzenia przepisu.";
        public const string RecipeErrorWhenDeleted = "Wystąpił błąd podczas usuwania przepisu.";
        public const string RecipeErrorWhenUpdate = "Wystąpił błąd podczas aktualizacji przepisu.";

        // Ingredient
        public const string IngredientErrorWhenCreated = "Wystąpił błąd podczas tworzenia składnika.";
        public const string IngredientCreateFails = "Błąd podczas tworzenia składników";
        //TimeTable
        public const string TimeTableCreateFails = "Błąd podczas tworzenia harmonogramu";

        public const string AuthenticationError = "Bad login and/or password.";
        public const string UserExist = "The user already exists.";
        public const string UserDoesntExist = "Podany użytkownik nie istnieje.";
        public const string RegisterDtoIsNotValid = "Data is not valid.";
        public const string TimetableDoesntExist = "The timmetable doesn't exist";
        public const string FileIsEmpty = "Przesłano pusty plik.";
        public const string FileWrongContentType = "Nieobsługiwany format danych.";
        public const string PhotoDoesntExist = "The photo doesnt exist.";

    }
}
