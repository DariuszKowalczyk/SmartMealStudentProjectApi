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
        Task<Responses<TimetableDto>> GetTimetablesByDay(DateTime day);
        Task<Response<TimetableDto>> CreateTimetable(TimetableBindingModel model);
        Task<Response<TimetableDto>> GetTimetableById(long id);
        Task<Response<DtoBaseModel>> DeleteTimetableById(long id);


    }
}
