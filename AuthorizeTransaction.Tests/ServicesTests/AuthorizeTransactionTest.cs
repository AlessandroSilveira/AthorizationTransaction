using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using AuthorizeTransaction.Domain.Services.Interfaces;
using AuthorizeTransaction.Domain.Services.AuthorizeTransactions;
using AuthorizeTransaction.Tests.MocksRepositories;
using System.Threading.Tasks;
using AuthorizeTransaction.Domain.Entities;
using System;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using System.Linq;

namespace AuthorizeTransaction.Test
{
    [TestFixture]
    public class AuthorizeTransactionTest
    {        
        private Mock<IAccountServices> _accountServiceMock;
        private Mock<ITransactionServices> _transactionServiceMock;       
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private AuthorizeTransactionService _authorizeTransactionService;
        private List<Record> Records;

        [SetUp] 
        public void SetUp()
        {
           
            _accountServiceMock = new Mock<IAccountServices>();
            _transactionServiceMock = new Mock<ITransactionServices>();           
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();    
            _authorizeTransactionService = new AuthorizeTransactionService(_accountServiceMock.Object, _transactionServiceMock.Object);
            Records = new List<Record>
            {
                new Record
                {
                    Id = 1,
                    Account = new Account
                    {
                        ActiveCard =true,
                        AvailableLimit = 10,
                        Id = 1

                    },
                    Transaction = new Transaction
                    {
                        Id = 1,
                        Amount = 10,
                        Merchant = "Teste",
                        Time = DateTime.Now
                    }
                }
            };
        }

        [Test]
        public async Task VerifyAuthorizeOperationsAsync()
        {            
                     
            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
           _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));
            _accountRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Account)).Returns(new FakeAccountRepository().UpdateAsync(Records.FirstOrDefault().Account));


            await _authorizeTransactionService.AuthorizeOperationsAsync(Records);

            _accountRepositoryMock.Verify();
            _transactionRepositoryMock.Verify();          
        }
    }
}
