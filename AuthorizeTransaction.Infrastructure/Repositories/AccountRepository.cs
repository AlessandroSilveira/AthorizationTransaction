using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Infrastructure.Context;
using AuthorizeTransaction.Infrastructure.Repositories.Base;

namespace AuthorizeTransaction.Infrastructure.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(AuthorizeTransactionContext context) : base(context)
        {
        }
    }
}
