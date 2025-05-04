using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class TGenericService<T> : ITGenericService<T> where T : class
    {
        protected readonly IGenericRepository<T> _genericRepository;

        public TGenericService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IReadOnlyList<T>> TGetAllAsync() 
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<T?> TGetByIdAsync(params object[] keyValues)
        {
            return await _genericRepository.GetByIdAsync(keyValues);
        }

        public async Task<IReadOnlyList<T>> TGetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _genericRepository.GetWhereAsync(predicate);
        }

        public async Task<T> TAddAsync(T entity)
        {
            return await _genericRepository.AddAsync(entity);
        }

        public async Task TUpdateAsync(T entity)
        {
            await _genericRepository.UpdateAsync(entity);
        }

        public async Task TDeleteAsync(T entity)
        {
            await _genericRepository.DeleteAsync(entity);
        }

        public async Task<int> TCountAsync()
        {
            return await _genericRepository.CountAsync();
        }
    }

}


