using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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
            var transaction1 = new Transaction();

            transaction1.Create(1, 10, "Teste", DateTime.Now);
            var transationsList = new List<Transaction>
            {
               transaction1
            };

            IEnumerable<Transaction> transactions = transationsList;

            return Task.FromResult(transactions);
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            var transaction = new Transaction();
            transaction.Create(1, 10, "Teste", DateTime.Now);
            
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

        internal Task<IEnumerable<Transaction>> GetAllAsyncForHighFrequency()
        {
            var transaction1 = new Transaction();
            var transaction2 = new Transaction();
            var transaction3 = new Transaction();
            var transaction4 = new Transaction();

            transaction1.Create(1, 150, merchant: "Burger King", new DateTime(2021, 08, 22, 16, 57, 00));
            transaction2.Create(2, 200, merchant: "Rapi", new DateTime(2021, 08, 22, 16, 57, 43));
            transaction3.Create(3, 200, merchant: "Uber Eats", new DateTime(2021, 08, 22, 16, 58, 44));
            transaction4.Create(4, 40, merchant: "Uber", new DateTime(2021, 08, 22, 16, 58, 45));

            var transactionList = new List<Transaction>
            {
               transaction1,
               transaction2,
               transaction3,
               transaction4
            };

            IEnumerable<Transaction> Transactions = transactionList;

            return Task.FromResult(Transactions);
        }

        internal Task<IEnumerable<Transaction>> GetAllAsyncForDoubleTransaction()
        {
            var transaction1 = new Transaction();
            var transaction2 = new Transaction();
            var transaction3 = new Transaction();

            transaction1.Create(1, 20, merchant: "Burger King", new DateTime(2021, 08, 22, 16, 57, 00));
            transaction2.Create(2, 10, merchant: "McDonald's", new DateTime(2021, 08, 22, 16, 57, 01));
            transaction3.Create(3, 15, merchant: "Burger King", new DateTime(2021, 08, 22, 16, 57, 02));

            var TransactionList = new List<Transaction>
            {
               transaction1,
               transaction2,
               transaction3

            };

            IEnumerable<Transaction> Transactions = TransactionList;

            return Task.FromResult(Transactions);
        }
    }
}
