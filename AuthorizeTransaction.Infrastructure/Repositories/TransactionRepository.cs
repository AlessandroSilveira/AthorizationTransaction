using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Infrastructure.Context;
using AuthorizeTransaction.Infrastructure.Repositories.Base;

namespace AuthorizeTransaction.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AuthorizeTransactionContext context) : base(context)
        {
        }
    }
}
