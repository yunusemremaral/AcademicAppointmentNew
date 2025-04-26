using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface ITGenericService<T> where T : class
    {
        Task<IReadOnlyList<T>> TGetAllAsync();
        Task<T?> TGetByIdAsync(params object[] keyValues);
        Task<IReadOnlyList<T>> TGetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<T> TAddAsync(T entity);
        Task TUpdateAsync(T entity);
        Task TDeleteAsync(T entity);
    }

}