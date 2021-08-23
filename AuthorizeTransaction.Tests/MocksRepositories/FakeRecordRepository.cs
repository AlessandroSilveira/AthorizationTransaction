using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.MocksRepositories
{
    public class FakeRecordRepository : IRecordRepository
    {
        public Task<Record> AddAsync(Record obj)
        {
            return Task.FromResult(obj);
        }

        public Task<IEnumerable<Record>> GetAllAsync()
        {
            var recordList = new List<Record>
            {
                new Record
                {
                    Id = 1,
                    Account = new Account
                    {
                        ActiveCard =true,
                        AvailableLimit = 10,
                        Id = 1,
                        // Violations = new List<string>()
                    },
                    Transaction = new Transaction
                    {
                        Id = 1,
                        Amount = 10,
                        Merchant = "Teste",
                        Time = DateTime.Now,
                        //Violations =  new List<string>()
                    }
                }
            };

            IEnumerable<Record> Records = recordList;

            return Task.FromResult(Records);
        }

        public Task<Record> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Record> UpdateAsync(Record obj)
        {
            return Task.FromResult( obj);
        }

        internal Task<IEnumerable<Record>> GetAllAsyncForHighFrequency()
        {
            var recordList = new List<Record>
            {
                new Record
                {
                    Id = 1,
                    Account = new Account
                    {
                        ActiveCard =true,
                        AvailableLimit = 1000,
                        Id = 1,
                        //Violations = new List<string>()
                    }
                },
                new Record
                {
                    Id = 2,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 150,
                            Merchant = "Burger King",
                            Time = new DateTime(2021,08,22,16,57,00),
                           // Violations =  new List<string>()
                        }
                },
                new Record
                {
                    Id = 3,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 200,
                            Merchant = "Rapi",
                            Time = new DateTime(2021,08,22,16,57,43),
                            //Violations =  new List<string>()
                        }
                },
                 new Record
                {
                    Id = 3,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 200,
                            Merchant = "Uber Eats",
                            Time = new DateTime(2021,08,22,16,58,44),
                            //Violations =  new List<string>()
                        }
                },
                  new Record
                {
                    Id = 4,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 40,
                            Merchant = "Uber",
                            Time = new DateTime(2021,08,22,16,58,45),
                            //Violations =  new List<string>()
                        }
                }


            };

            IEnumerable<Record> Records = recordList;

            return Task.FromResult(Records);
        }

        internal Task<IEnumerable<Record>> GetAllAsyncForDoubleTransaction()
        {
            var recordList = new List<Record>
            {
                new Record
                {
                    Id = 1,
                    Account = new Account
                    {
                        ActiveCard =true,
                        AvailableLimit = 100,
                        Id = 1,
                        //Violations = new List<string>()
                    }
                },
                new Record
                {
                    Id = 2,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 20,
                            Merchant = "Burger King",
                            Time = new DateTime(2021,08,22,16,57,00),
                            //Violations =  new List<string>()
                        }
                },
                new Record
                {
                    Id = 3,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 10,
                            Merchant = "McDonald's",
                            Time = new DateTime(2021,08,22,16,57,01),
                            //Violations =  new List<string>()
                        }
                },
                 new Record
                {
                    Id = 3,
                    Transaction = new Transaction
                        {
                            Id = 1,
                            Amount = 15,
                            Merchant = "Burger King",
                            Time = new DateTime(2021,08,22,16,57,02),
                            //Violations =  new List<string>()
                        }
                 }


            };

            IEnumerable<Record> Records = recordList;

            return Task.FromResult(Records);
        }
    }
}
