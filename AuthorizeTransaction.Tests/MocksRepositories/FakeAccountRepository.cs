using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.MocksRepositories
{
    public class FakeAccountRepository : IAccountRepository
    {
        public Task<Account> AddAsync(Account obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAsync()
        {
            var accountList = new List<Account>
            {
                new Account
                {
                    ActiveCard = true,
                    AvailableLimit = 10,
                    Id = 1,
                    //Violations = new List<string>()
                }
            };

            IEnumerable<Account> result = accountList.ToList(); 
            return Task.FromResult(result);
        }

        public Task<Account> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Account> UpdateAsync(Account obj)
        {
            return Task.FromResult(obj);
        }

        public  Task<IEnumerable<Account>> GetAllWithCardNotActivated()
        {
            var accountList = new List<Account>
            {
                new Account
                {
                    ActiveCard = false,
                    AvailableLimit = 10,
                    Id = 1,
                    //Violations = new List<string>()
                }
            };

            IEnumerable<Account> result = accountList.ToList();
            return Task.FromResult(result);
        }

        internal Task<IEnumerable<Account>> GetAllAsyncWithOneAccount()
        {
            var accountList = new List<Account>
            {
                new Account
                {
                    ActiveCard = true,
                    AvailableLimit = 100,
                    Id = 1,
                    //Violations = new List<string>()
                }
            };

            IEnumerable<Account> result = accountList.ToList();
            return Task.FromResult(result);
        }

        internal Task<IEnumerable<Account>> GetAllWithAccountNull()
        {
            var accountList = new List<Account>();
           
            IEnumerable<Account> result = accountList.ToList();
            return Task.FromResult(result);
        }

        
    }
}
