using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Transactions;
using AuthorizeTransaction.Tests.MocksRepositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.ServicesTests
{
    [TestFixture]
    public class TransactionServiceTest
    {
       
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private TransactionServices _transactionServices;
        private List<Record> Records;
        private Violation violation;
        private Record record;

        [SetUp]
        public void SetUp() 
        {
           
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionServices = new TransactionServices(_transactionRepositoryMock.Object, _accountRepositoryMock.Object);
            var transaction = new Transaction();
            transaction.Create(1,10, "Teste",DateTime.Now);
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
                    Transaction = transaction
                }
            };
            violation = new Violation();
            record = new Record
            {
                Transaction = transaction
            };
        }

        [Test]
        public async Task TransactionAuthorizationWhenReceiveTransactionRecordAsync()
        {
            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));

            var response = await _transactionServices.TransactionAuthorizationAsync(record,  violation.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();

        }

        [Test]
        public async Task TransactionAuthorizationShouldReturnCardNotActicated()
        {
            Records.FirstOrDefault().Account.ActiveCard = false;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllWithCardNotActivated());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));

            var response = await _transactionServices.TransactionAuthorizationAsync(record,  violation.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
            violation.Violations.Should().Contain("card-not-active");

        }
        [Test]
        public async Task TransactionAuthorizationShouldReturnCardNotActicatedIfAccountIsNull()
        {
            Records.FirstOrDefault().Account = null;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllWithAccountNull());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));

            var response = await _transactionServices.TransactionAuthorizationAsync(record,  violation.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
            violation.Violations.Should().Contain("account-not-initialized");

        }


        [Test]
        public async Task TransactionAuthorizationWhenReceiveTransactionShouldReturnHighFrequencySmallInterval()
        {
            var transaction = Records.FirstOrDefault().Transaction;
            Records.FirstOrDefault().Account.AvailableLimit = 1000;
            transaction.Create(1, 100, "Burger King", new DateTime(2021, 08, 22, 16, 58, 55));
           

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(transaction.Id));
            _transactionRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeTransactionRepository().GetAllAsyncForHighFrequency());
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault(), violation.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
            violation.Violations.Should().Contain("high-frequency-small-interval");

        }

        [Test]
        public async Task TransactionAuthorizationShouldReturnDoubleTransaction()
        {
            
            var transaction = Records.FirstOrDefault().Transaction;
            transaction.Create(1, 20, "Burguer King", new DateTime(2021, 08, 22, 16, 57, 03));
           
            Records.FirstOrDefault().Transaction = transaction;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(transaction.Id));
            _transactionRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeTransactionRepository().GetAllAsyncForDoubleTransaction());
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());

            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault(), violation.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
            violation.Violations.Should().Contain("doubled-transaction");

        }

        [Test]
        public async Task TransactionAuthorizationShouldReturnInsufficientLimit()
        {
            Records.FirstOrDefault().Account.ActiveCard = true;
            Records.FirstOrDefault().Account.AvailableLimit = 10;
            var transaction = Records.FirstOrDefault().Transaction;

            transaction.Create(1, 20, "Burguer King", new DateTime(2021, 08, 22, 16, 57, 03));

            Records.FirstOrDefault().Transaction = transaction;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(transaction.Id));           
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsyncWithInsufficientLimit());
            var response = await _transactionServices.TransactionAuthorizationAsync(record, violation.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
            violation.Violations.Should().Contain("insufficient-limit");

        }
    }
}
