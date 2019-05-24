using AutoMapper;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMeal.Service.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly IRepository<Timetable> _timetableRepository;
        private readonly IRepository<Recipe> _recipeRepository;
        public readonly IRepository<User> _userRepository;

        public TimetableService(IRepository<Timetable> timetableRepository, IRepository<User> userRepository, IRepository<Recipe> recipeRepository)
        {
            _timetableRepository = timetableRepository;
            _userRepository = userRepository;
            _recipeRepository = recipeRepository;
        }

        public async Task<Responses<TimetableDto>> GetTimetablesByDay(DateTime day, long userId)
        {
            var resposne = new Responses<TimetableDto>();
            var timetables = await _timetableRepository.GetAllByAsync(x => x.MealDay == day && x.Owner.Id == userId, includes: x => x.Recipe.Image);
            var timetableDtos = new List<TimetableDto>();
            foreach (var timetable in timetables)
            {
                var timetableDto = Mapper.Map<TimetableDto>(timetable);
                timetableDtos.Add(timetableDto);
            }
            resposne.Data = timetableDtos;
            return resposne;
        }

        public async Task<Response<TimetableDto>> CreateTimetable(TimetableBindingModel model, long userId)
        {
            var response = new Response<TimetableDto>();

            var recipe = await _recipeRepository.GetByAsync(x => x.Id == model.RecipeId, withTracking:true);

            if (recipe == null)
            {
                response.AddError(Error.RecipeDoesntExist);
                return response;
            }
            var user = await _userRepository.GetByAsync(x => x.Id == userId, withTracking: true);
            if (user == null)
            {
                response.AddError(Error.UserDoesntExist);
                return response;
            }
            var timetable = Mapper.Map<Timetable>(model);
            timetable.Recipe = recipe;
            timetable.Owner = user;

            bool isCreated = await _timetableRepository.CreateAsync(timetable);

            if (isCreated)
            {
                var timetableDto = Mapper.Map<TimetableDto>(timetable);
                response.Data = timetableDto;
                return response;
            }
            else
            { 
                response.AddError(Error.TimeTableCreateFails);
                return response;
            }


        }

        public async Task<Response<TimetableDto>> GetTimetableById(long id, long userId)
        {
            var response = new Response<TimetableDto>();
            var timetable = await _timetableRepository.GetByAsync(x => x.Id == id && x.Owner.Id == userId, includes: param => param.Recipe.Image);
            if (timetable == null)
            {
                response.AddError(Error.TimetableDoesntExist);
                return response;
            }

            var timetableDto = Mapper.Map<TimetableDto>(timetable);
            response.Data = timetableDto;

            return response;
        }

        public async Task<Response<DtoBaseModel>> DeleteTimetableById(long id, long userId)
        {
            var response = new Response<DtoBaseModel>();

            var timetable = await _timetableRepository.GetByAsync(x => x.Id == id && x.Owner.Id == userId);
            if (timetable == null)
            {
                response.AddError(Error.TimetableDoesntExist);
                return response;
            }

            bool IsDeleted = await _timetableRepository.RemoveElement(timetable);
            if (!IsDeleted)
            {
                response.AddError(Error.TimeTableCreateFails);
                return response;
            }

            return response;
        }
    }
}
