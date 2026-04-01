using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tarcking = true);
        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, bool tarcking = true);
        public Task Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }

}