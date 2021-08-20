using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Services.Interfaces;

namespace AuthorizeTransaction.Domain.Services.Account
{
    public class AccountServices : IAccountServices
    {   public Entities.Account AccountCreationAsync(Entities.Account account)
        {
            if (account.ActiveCard)
                account.Violations.Add("account-already-initialized");
            else
            {
                account.ActiveCard = account.ActiveCard;
                account.AvailableLimit = account.AvailableLimit;
            }

            return account;
        } 

       
    }
}
