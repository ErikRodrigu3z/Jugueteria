using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jugueteria.Service.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        Task<List<T>> GetListAsync(); 
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        void Create(T entity); 
        Task CreateAsync(T entity);
        void Update(T entity); 
        Task UpdateAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity); 


    }
}
