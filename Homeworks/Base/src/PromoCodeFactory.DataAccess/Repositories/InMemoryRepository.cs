using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public async Task<bool> AddAsync(T entity)
        {
            var isEntityContains = Data.Contains(entity);
            if (!isEntityContains)
            {
                (Data as List<T>).Add(entity);
            }
            return await Task.FromResult(isEntityContains);
        }

        public async Task<bool> RemoveAsync(Guid entityId)
        {
            var entity = Data.FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                (Data as List<T>).Remove(entity);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await RemoveAsync(entity.Id);
            await AddAsync(entity);
            return await Task.FromResult(entity);
        }
    }
}