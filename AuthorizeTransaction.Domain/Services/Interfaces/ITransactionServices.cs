using AuthorizeTransaction.Domain.Entities;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface ITransactionServices
    {
        Record? TransactionAuthorization(Record? item);
    }
}
