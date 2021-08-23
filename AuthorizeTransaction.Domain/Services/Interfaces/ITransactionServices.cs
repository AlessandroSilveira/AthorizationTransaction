using AuthorizeTransaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface ITransactionServices
    {
        Task<Entities.Account> TransactionAuthorizationAsync(Record? item, List<string> violations);
    }
}
