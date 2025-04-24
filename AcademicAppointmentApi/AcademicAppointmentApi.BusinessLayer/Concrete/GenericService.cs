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
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IGenericRepository<T> _genericRepository;

        public GenericService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<T>> TGetAllAsync() => await _genericRepository.GetAllAsync();
        public async Task<T> TGetByIdAsync(int id) => await _genericRepository.GetByIdAsync(id);
        public async Task<List<T>> TGetWhereAsync(Expression<Func<T, bool>> predicate) => await _genericRepository.GetWhereAsync(predicate);
        public async Task TAddAsync(T entity) => await _genericRepository.AddAsync(entity);
        public async Task TUpdateAsync(T entity) => _genericRepository.Update(entity);
        public async Task TDeleteAsync(T entity) => _genericRepository.Delete(entity);
        public async Task<int> TSaveAsync() => await _genericRepository.SaveAsync();
        public async Task<T> TGetByIdWithStringAsync(string id) => await _genericRepository.GetByIdWithStringAsync(id);
    }
}


