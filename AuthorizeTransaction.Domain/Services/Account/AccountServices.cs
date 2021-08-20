using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var accountRepo = await _accountRepository.GetAllAsync();

            var accountStored = accountRepo.ToList().FirstOrDefault();

            if (accountStored != null)
            {
                if (account.ActiveCard == true && accountStored.ActiveCard == true)
                    accountStored.Violations.Add("account-already-initialized");                   

                if (account.AvailableLimit == accountStored.AvailableLimit && account.ActiveCard == accountStored.ActiveCard)
                    accountStored.Violations.Add("doubled-transaction");

                await _accountRepository.UpdateAsync(accountStored);

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

        public async Task<Entities.Account> GetAccountAsync()
        {
            IEnumerable<Entities.Account> accountRepo = await _accountRepository.GetAllAsync();
            Entities.Account accountStored = accountRepo.ToList().FirstOrDefault();

            return accountStored;
        }
    }
}
