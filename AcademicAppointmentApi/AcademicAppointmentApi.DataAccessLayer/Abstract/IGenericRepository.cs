using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdWithStringAsync(string id);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveAsync();
    }

}
