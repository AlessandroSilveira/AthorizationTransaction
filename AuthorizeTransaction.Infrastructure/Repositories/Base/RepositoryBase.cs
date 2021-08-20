using AuthorizeTransaction.Domain.Repositories.Interfaces.Base;
using AuthorizeTransaction.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Infrastructure.Repositories.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected AuthorizeTransactionContext _context;

        public RepositoryBase(AuthorizeTransactionContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(obj);
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException("Não foi possível realizar a operação de inserção no banco", ex);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {

            _context.Entry(obj).State = EntityState.Detached;
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

        }
    }
}
