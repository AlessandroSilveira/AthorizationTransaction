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
using System.Text;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.ServicesTests
{
    [TestFixture]
    public class TransactionServiceTest
    {
        private Mock<IRecordRepository> _recordRepositoryMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private TransactionServices _transactionServices;
        private List<Record> Records;

        [SetUp]
        public void SetUp() 
        {
            _recordRepositoryMock = new Mock<IRecordRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionServices = new TransactionServices(_recordRepositoryMock.Object, _transactionRepositoryMock.Object, _accountRepositoryMock.Object);
            Records = new List<Record>
            {
                new Record
                {
                    Id = 1,
                    Account = new Account
                    {
                        ActiveCard =true,
                        AvailableLimit = 10,
                        Id = 1,
                        Violations = new List<string>()
                    },
                    Transaction = new Transaction
                    {
                        Id = 1,
                        Amount = 10,
                        Merchant = "Teste",
                        Time = DateTime.Now,
                        Violations =  new List<string>()
                    }
                }
            };
        }

        [Test]
        public async Task TransactionAuthorizationWhenReceiveTransactionRecordAsync()
        {
            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));

            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault());

            response.Should().NotBeNull();
            response.Should().BeOfType<Record>();

        }

        [Test]
        public async Task TransactionAuthorizationShouldReturnCardNotActicated()
        {
            Records.FirstOrDefault().Account.ActiveCard = false;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllWithCardNotActivated());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));

            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault());

            response.Should().NotBeNull();
            response.Should().BeOfType<Record>();
            response.Transaction.Violations.Should().Contain("card-not-active");

        }
        [Test]
        public async Task TransactionAuthorizationShouldReturnCardNotActicatedIfAccountIsNull()
        {
            Records.FirstOrDefault().Account = null;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(Records.FirstOrDefault().Transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(Records.FirstOrDefault().Transaction.Id));
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllWithAccountNull());
            _transactionRepositoryMock.Setup(a => a.UpdateAsync(Records.FirstOrDefault().Transaction)).Returns(new FakeTransactionRepository().UpdateAsync(Records.FirstOrDefault().Transaction));

            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault());

            response.Should().NotBeNull();
            response.Should().BeOfType<Record>();
            response.Transaction.Violations.Should().Contain("account-not-initialized");

        }


        [Test]
        public async Task TransactionAuthorizationWhenReceiveTransactionShouldReturnHighFrequencySmallInterval()
        {
            var transaction = Records.FirstOrDefault().Transaction;

            transaction.Id = 1;
            transaction.Merchant = "Burger King";
            transaction.Time = new DateTime(2021, 08, 22, 16, 58, 55);
            transaction.Amount = 100;

            Records.FirstOrDefault().Transaction = transaction;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(transaction.Id));
            _recordRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeRecordRepository().GetAllAsyncForHighFrequency());
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault());

            response.Should().NotBeNull();
            response.Should().BeOfType<Record>();
            response.Transaction.Violations.Should().Contain("high-frequency-small-interval");

        }

        [Test]
        public async Task TransactionAuthorizationShouldReturnDoubleTransaction()
        {
            Records.FirstOrDefault().Account.ActiveCard = true;
            var transaction = Records.FirstOrDefault().Transaction;

            transaction.Id = 1;
            transaction.Merchant = "Burger King";
            transaction.Time = new DateTime(2021, 08, 22, 16, 57, 03);
            transaction.Amount = 20;

            Records.FirstOrDefault().Transaction = transaction;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(transaction.Id));
            _recordRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeRecordRepository().GetAllAsyncForDoubleTransaction());
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault());

            response.Should().NotBeNull();
            response.Should().BeOfType<Record>();
            response.Transaction.Violations.Should().Contain("doubled-transaction");

        }

        [Test]
        public async Task TransactionAuthorizationShouldReturnInsufficientLimit()
        {
            Records.FirstOrDefault().Account.ActiveCard = true;
            Records.FirstOrDefault().Account.AvailableLimit = 10;
            var transaction = Records.FirstOrDefault().Transaction;

            transaction.Id = 1;
            transaction.Merchant = "Burger King";
            transaction.Time = new DateTime(2021, 08, 22, 16, 57, 03);
            transaction.Amount = 20;

            Records.FirstOrDefault().Transaction = transaction;

            _transactionRepositoryMock.Setup(a => a.GetByIdAsync(transaction.Id)).Returns(new FakeTransactionRepository().GetByIdAsync(transaction.Id));
            _recordRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeRecordRepository().GetAllAsync());
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            var response = await _transactionServices.TransactionAuthorizationAsync(Records.FirstOrDefault());

            response.Should().NotBeNull();
            response.Should().BeOfType<Record>();
            response.Transaction.Violations.Should().Contain("insufficient-limit");

        }
    }
}
