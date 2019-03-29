using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;

namespace SmartMeal.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public Task<bool> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
