using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        Task<List<T>> TGetAllAsync();
        Task<T> TGetByIdAsync(int id);
        Task<T> TGetByIdWithStringAsync(string id);
        Task<List<T>> TGetWhereAsync(Expression<Func<T, bool>> predicate);
        Task TAddAsync(T entity);
        Task TUpdateAsync(T entity);
        Task TDeleteAsync(T entity);
        Task<int> TSaveAsync();
    }

}