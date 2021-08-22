using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.MocksRepositories
{
    public class FakeTransactionRepository : ITransactionRepository
    {
        public Task<Transaction> AddAsync(Transaction obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllAsync()
        {
            var transationsList = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 10,
                    Id = 1,
                    Merchant = "Teste",
                    Time = DateTime.Now,
                    Violations = new List<string>()
                }
            };

            IEnumerable<Transaction> transactions = transationsList;

            return Task.FromResult(transactions);   
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            var transaction = new Transaction
            {
                Amount = 10,
                Id = 1,
                Merchant = "Teste",
                Time = DateTime.Now,
                Violations = new List<string>()
            };
            return Task.FromResult(transaction);    
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> UpdateAsync(Transaction obj)
        {
            return Task.FromResult(obj);
        }
    }
}
