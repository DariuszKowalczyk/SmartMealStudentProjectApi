using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly IRepository<Timetable> _timetableRepository;
//        private readonly IRecipeService _recipeService;
        private readonly IRepository<Recipe> _recipeRepository;

        public TimetableService(IRepository<Timetable> timetableRepository, IRepository<Recipe> recipeRepository)
        {
            _timetableRepository = timetableRepository;
            _recipeRepository = recipeRepository;
        }


        public async Task<Responses<TimetableDto>> GetTimetablesByDay(DateTime day)
        {
            var resposne = new Responses<TimetableDto>();
            var timetables = await _timetableRepository.GetAllByAsync(x => x.MealDay == day, includes: x => x.Recipe);
            var timetableDtos = new List<TimetableDto>();
            foreach (var timetable in timetables)
            {
                var timetableDto = Mapper.Map<TimetableDto>(timetable);
                timetableDtos.Add(timetableDto);
            }

            resposne.Data = timetableDtos;
            return resposne;
        }

        public async Task<Response<TimetableDto>> CreateTimetable(TimetableBindingModel model)
        {
            var response = new Response<TimetableDto>();

            var recipe = await _recipeRepository.GetByAsync(x => x.Id == model.RecipeId, withTracking:true);

            if (recipe == null)
            {
                response.AddError(Error.RecipeDoesntExist);
                return response;
            }

            var timetable = Mapper.Map<Timetable>(model);
            timetable.Recipe = recipe;

            bool isCreated = await _timetableRepository.CreateAsync(timetable);

            if (isCreated)
            {
                var timetableDto = Mapper.Map<TimetableDto>(timetable);
                response.Data = timetableDto;
                return response;
            }

            return response;

        }

        public async Task<Response<TimetableDto>> GetTimetableById(long id)
        {
            var response = new Response<TimetableDto>();
            var timetable = await _timetableRepository.GetByAsync(x => x.Id == id);
            if (timetable == null)
            {
                response.AddError(Error.TimetableDoesntExist);
                return response;
            }

            var timetableDto = Mapper.Map<TimetableDto>(timetable);
            response.Data = timetableDto;

            return response;
        }

        public async Task<Response<DtoBaseModel>> DeleteTimetableById(long id)
        {
            var response = new Response<DtoBaseModel>();

            var timetable = await _timetableRepository.GetByAsync(x => x.Id == id);
            if (timetable == null)
            {
                response.AddError(Error.TimetableDoesntExist);
                return response;
            }

            bool IsDeleted = await _timetableRepository.RemoveElement(timetable);
            if (!IsDeleted)
            {
                response.AddError("Błąd podczas usuwania harmonogramu");
                return response;
            }

            return response;
        }
    }
}
