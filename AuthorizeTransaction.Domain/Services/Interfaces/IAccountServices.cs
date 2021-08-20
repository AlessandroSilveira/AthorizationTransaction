using AuthorizeTransaction.Domain.Entities;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface IAccountServices
    {
        Task<Entities.Account> AccountCreationAsync(Entities.Account account);
        Task<Entities.Account> GetAccountAsync();
    }
}
 