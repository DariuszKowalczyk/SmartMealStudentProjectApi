using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.Models;

namespace SmartMeal.Data.Repository.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<bool> CreateAsync(T entity);
    }
}
