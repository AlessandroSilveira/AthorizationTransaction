using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Interfaces;

namespace AuthorizeTransaction.Domain.Services.Account
{
    public class AccountServices : IAccountServices
    {   
        private readonly IAccountRepository _accountRepository;

        public AccountServices(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Entities.Account> AccountCreationAsync(Entities.Account account)
        {
            var accountRepo = _accountRepository.GetAllAsync();

            IEnumerable<Entities.Account> accountrepo = await _accountRepository.GetAllAsync();

            Entities.Account accountStored = accountrepo.ToList().FirstOrDefault();

            if (accountStored != null)
            {
                if (account.ActiveCard == true && accountStored.ActiveCard == true)
                {
                    accountStored.Violations.Add(new Violations { Violation = "account-already-initialized" });
                    await _accountRepository.UpdateAsync(accountStored);
                }

                if(account.AvailableLimit == accountStored.AvailableLimit && account.ActiveCard == accountStored.ActiveCard)
                {
                    accountStored.Violations.Add(new Violations { Violation = "doubled-transaction" });
                    _accountRepository.UpdateAsync(accountStored);  
                }

                return accountStored;
            }
            else
            {
                account.ActiveCard = account.ActiveCard;
                account.AvailableLimit = account.AvailableLimit;
                await _accountRepository.AddAsync(account);
                return account;
            }


        }
    }
}
