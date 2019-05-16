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

        public List<string> Errors { get; set; }

        public Response()
        {
            Errors = new List<string>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

    }

    public class Responses<T> where T : DtoBaseModel
    {
        public List<T> Data { get; set; }

        public bool IsError;

        public List<string> Errors { get; set; }

        public Responses()
        {
            Errors = new List<string>();
            Data = new List<T>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }


    }
}
