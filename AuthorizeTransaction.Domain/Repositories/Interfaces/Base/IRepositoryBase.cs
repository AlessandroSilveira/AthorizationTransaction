using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Repositories.Interfaces.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity obj);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> UpdateAsync(TEntity obj);

        Task RemoveAsync(int id);

        Task<TEntity> GetByIdAsync(int id);


    }
}
