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

        private Mock<IRecordServices> _recordServiceMock;
        private Mock<IAccountServices> _accountServiceMock;
        private Mock<ITransactionServices> _transactionServiceMock;
        private Mock<IRecordRepository> _recordRepositoryMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private AuthorizeTransactionService _authorizeTransactionService;
        private List<Record> Records;

        [SetUp] 
        public void SetUp()
        {
            _recordServiceMock =    new Mock<IRecordServices>();
            _accountServiceMock = new Mock<IAccountServices>();
            _transactionServiceMock = new Mock<ITransactionServices>();
            _recordRepositoryMock = new Mock<IRecordRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();    
            _authorizeTransactionService = new AuthorizeTransactionService(_recordServiceMock.Object, _accountServiceMock.Object, _transactionServiceMock.Object);
             
        }

        [Test]
        public void VerifyAuthorizeOperationsAsync()
        {
            _recordServiceMock.Setup(a => a.GetAll()).Returns(Task.FromResult(Records));
            _recordRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeRecordRepository().GetAllAsync());
            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));
            _accountRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Account)).Returns(new FakeAccountRepository().UpdateAsync(Records.FirstOrDefault().Account));


            _authorizeTransactionService.AuthorizeOperationsAsync();

            _recordServiceMock.Verify(mock => mock.GetAll());


        }
    }
}
