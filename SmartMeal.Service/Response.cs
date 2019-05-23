using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service
{
    public class Response<T> where T : DtoBaseModel
    {
        public T Data { get; set; }

        public bool IsError => Errors.Count > 0;

        public bool IsValid => Data != null;

        public List<ErrorDto> Errors { get; set; }

        public Response()
        {
            Errors = new List<ErrorDto>();
        }

        public void AddError(string error)
        {
            Errors.Add(new ErrorDto()
            {
                Message = error
            });
        }

    }

    public class Responses<T> where T : DtoBaseModel
    {
        public List<T> Data { get; set; }

        public bool IsError => Errors.Count > 0;

        public List<ErrorDto> Errors { get; set; }

        public Responses()
        {
            Errors = new List<ErrorDto>();
            Data = new List<T>();
        }

        public void AddError(string error)
        {
            Errors.Add(new ErrorDto()
            {
                Message = error
            });
        }


    }
}
