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
                    Time = DateTime.Now

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
                Time = DateTime.Now

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

        internal Task<IEnumerable<Transaction>> GetAllAsyncForHighFrequency()
        {
            var transactionList = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    Amount = 150,
                    Merchant = "Burger King",
                    Time = new DateTime(2021,08,22,16,57,00)
                },
                new Transaction
                {
                    Id = 1,
                    Amount = 200,
                    Merchant = "Rapi",
                    Time = new DateTime(2021,08,22,16,57,43)
                },
                new Transaction
                {
                    Id = 1,
                    Amount = 200,
                    Merchant = "Uber Eats",
                    Time = new DateTime(2021,08,22,16,58,44)
                },
                new Transaction
                {
                    Id = 1,
                    Amount = 40,
                    Merchant = "Uber",
                    Time = new DateTime(2021,08,22,16,58,45)
                }
            };

            IEnumerable<Transaction> Transactions = transactionList;

            return Task.FromResult(Transactions);
        }

        internal Task<IEnumerable<Transaction>> GetAllAsyncForDoubleTransaction()
        {
            var TransactionList = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    Amount = 20,
                    Merchant = "Burger King",
                    Time = new DateTime(2021,08,22,16,57,00),

                },
                new Transaction
                {
                    Id = 1,
                    Amount = 10,
                    Merchant = "McDonald's",
                    Time = new DateTime(2021,08,22,16,57,01)
                 },
               new Transaction
               {
                    Id = 1,
                    Amount = 15,
                    Merchant = "Burger King",
                    Time = new DateTime(2021,08,22,16,57,02)
               }
            };

            IEnumerable<Transaction> Transactions = TransactionList;

            return Task.FromResult(Transactions);
        }
    }
}
