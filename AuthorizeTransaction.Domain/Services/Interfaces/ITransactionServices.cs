using AuthorizeTransaction.Domain.Entities;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface ITransactionServices
    {
        Task<Record> TransactionAuthorizationAsync(Record? item);
    }
}
