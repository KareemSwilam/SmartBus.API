using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        
        protected readonly ApplicationContext _context;
        protected DbSet<T> _dbSet; 
        public Repository(ApplicationContext context)
        {
            _context = context; 
            _dbSet = _context.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);  
        }

        public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tarcking = true)
        {
           IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tarcking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, bool tarcking = true)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tarcking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
