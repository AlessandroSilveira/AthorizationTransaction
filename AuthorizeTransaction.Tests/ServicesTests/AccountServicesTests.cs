using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Account;
using AuthorizeTransaction.Tests.MocksRepositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.ServicesTests
{
    [TestFixture]
    public class AccountServicesTests
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private AccountServices _services;
        private Account account;
        private Violation violations;

        [SetUp]
        public void SetUp()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _services = new AccountServices(_accountRepositoryMock.Object);
            account = new Account
            {
                ActiveCard = true,
                AvailableLimit = 100,
               // Violations = new List<string>()
            };

            violations = new Violation();
        }

        [Test]
        public async Task AccountCreationAsyncShouldReturnAccountAsync()
        {

            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllWithAccountNull());
            _accountRepositoryMock.Setup(a => a.UpdateAsync(account)).Returns(new FakeAccountRepository().UpdateAsync(account));

            var response = await _services.AccountCreationAsync(account, violations.Violations);    

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
        }

        [Test]
        public async Task AccountCreationAsyncShouldReturnAccountAlreadyInitialized()
        {
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsyncWithOneAccount());
            _accountRepositoryMock.Setup(a => a.UpdateAsync(account)).Returns(new FakeAccountRepository().UpdateAsync(account));

            var response = await _services.AccountCreationAsync(account, violations.Violations);

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
            violations.Violations.Should().Contain("account-already-initialized");
        }

        [Test]
        public async Task AccountCreationAsyncShouldReturnDoubledTransaction()
        {
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsyncWithOneAccount());
            _accountRepositoryMock.Setup(a => a.UpdateAsync(account)).Returns(new FakeAccountRepository().UpdateAsync(account));

            var response = await _services.AccountCreationAsync(account, new List<string>());

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
           // response.Violations.Should().Contain("doubled-transaction");
        }


        [Test]
        public async Task GetAccountAsyncShouldReturnAccount()
        {
            _accountRepositoryMock.Setup(a => a.GetAllAsync()).Returns(new FakeAccountRepository().GetAllAsync());
            
            var response = await _services.GetAccountAsync();

            response.Should().NotBeNull();
            response.Should().BeOfType<Account>();
        }
    }
}
