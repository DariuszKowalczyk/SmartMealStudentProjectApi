using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface ITimetableService
    {
        Task<Responses<TimetableDto>> GetTimetablesByDay(DateTime day, long userId);
        Task<Response<TimetableDto>> CreateTimetable(TimetableBindingModel model, long userId);
        Task<Response<TimetableDto>> GetTimetableById(long id, long userId);
        Task<Response<DtoBaseModel>> DeleteTimetableById(long id, long userId);


    }
}
