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

        public async Task<Entities.Account> AccountCreationAsync(Entities.Account account, List<string> violations)
        {

            var accountRepo = await _accountRepository.GetAllAsync();
            var accountStored = accountRepo.ToList();

            if(accountStored.Count == 0)
            {
                await _accountRepository.AddAsync(account);
                return account;
            }
            else
            {
                var violatedAccount = accountStored.FirstOrDefault();

                if(!violations.Contains("account-already-initialized"))
                    violations.Add("account-already-initialized");

                await _accountRepository.UpdateAsync(violatedAccount);
                return violatedAccount;
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
