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

            var accountStored = accountRepo.ToList();
            if (accountStored.Count() > 1)
            {
                if (accountStored != null)
                {
                    if (accountStored.Where(a => a.ActiveCard == true).Count() > 1)
                        account.Violations.Add("account-already-initialized");
                    foreach (var item in accountStored)
                    {
                        if (accountStored.Any(a => a.ActiveCard == item.ActiveCard && a.AvailableLimit == item.AvailableLimit))
                        {
                            if(!account.Violations.Contains("doubled-transaction"))
                                account.Violations.Add("doubled-transaction");
                        }
                    }

                    await _accountRepository.UpdateAsync(account);

                    return account;
                }
                else
                {
                    account.ActiveCard = account.ActiveCard;
                    account.AvailableLimit = account.AvailableLimit;
                    await _accountRepository.AddAsync(account);

                    return account;
                }
                

               
            }
            return account;

        }

        public async Task<Entities.Account> GetAccountAsync()
        {
            IEnumerable<Entities.Account> accountRepo = await _accountRepository.GetAllAsync();
            Entities.Account accountStored = accountRepo.ToList().FirstOrDefault();

            return accountStored;
        }
    }
}
