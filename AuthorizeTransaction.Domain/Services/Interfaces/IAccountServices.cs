using AuthorizeTransaction.Domain.Entities;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface IAccountServices
    {
        Entities.Account AccountCreationAsync(Entities.Account account);
       
    }
}
