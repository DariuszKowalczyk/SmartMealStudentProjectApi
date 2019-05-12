using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;

namespace SmartMeal.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> RemoveElement(T entity)
        {
            _dbSet.Remove(entity);
            return await SaveAsync();

        }

        public async Task<bool> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await SaveAsync();
            
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<bool> AnyExist(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        private async Task<bool> SaveAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }
    }
}
