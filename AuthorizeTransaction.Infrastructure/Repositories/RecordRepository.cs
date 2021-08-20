using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Infrastructure.Context;
using AuthorizeTransaction.Infrastructure.Repositories.Base;

namespace AuthorizeTransaction.Infrastructure.Repositories
{

    public class RecordRepository : RepositoryBase<Record>, IRecordRepository
    {
        public RecordRepository(AuthorizeTransactionContext context) : base(context)
        {
        }
        
       
    }
}
