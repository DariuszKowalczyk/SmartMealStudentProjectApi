﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.Models;

namespace SmartMeal.Data.Repository.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<bool> CreateAsync(T entity);
        Task<T> GetByAsync(Expression<Func<T, bool>> expression, bool withTracking = false, params Expression<Func<T, object>>[] includes);
        Task<bool> AnyExist(Expression<Func<T, bool>> expression);
        Task<bool> RemoveElement(T entity);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllByAsync(Expression<Func<T, bool>> expression, bool withTracking = false, params Expression<Func<T, object>>[] includes);
        Task<bool> UpdateAsync(T entity);
        Task<bool> CreateRangeAsync(IEnumerable<T> entities);

    }
}
