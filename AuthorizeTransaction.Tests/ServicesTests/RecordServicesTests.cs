using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Records;
using AuthorizeTransaction.Tests.MocksRepositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Tests.ServicesTests
{
    [TestFixture]
    public class RecordServicesTests
    {
        private Mock<IRecordRepository> _recordRepositoryMock;
        private RecordServices _services;
        private Record record;

        [SetUp]
        public void Setup()
        {
            _recordRepositoryMock = new Mock<IRecordRepository>();
            record = new Record
            {
                Id = 1,
                Account = new Account
                {
                    Id = 1,
                    AvailableLimit = 1000,
                    ActiveCard = true,
                    Violations = new List<string>()
                }
            };
                    
            _services = new RecordServices(_recordRepositoryMock.Object);   
        }

        [Test]
        public async Task VerifyAddAsync()
        {
            _recordRepositoryMock.Setup(r => r.AddAsync(record)).Returns(new FakeRecordRepository().AddAsync(record));

            await _services.AddAsync(record);

            _recordRepositoryMock.Verify(r => r.AddAsync(record));
        }

        [Test]
        public async Task GetAllShouldReturnListOfRecords()
        {
            _recordRepositoryMock.Setup(r => r.GetAllAsync()).Returns(new FakeRecordRepository().GetAllAsync());

           var response = await _services.GetAll();

            response.Should().HaveCount(1);
            response.Should().NotBeNull();              
        }

        [Test]
        public async Task VerifyUpdateAsync()
        {
            _recordRepositoryMock.Setup(r => r.UpdateAsync(record)).Returns(new FakeRecordRepository().UpdateAsync(record));

             _services.Update(record);

            _recordRepositoryMock.Verify(r => r.UpdateAsync(record));
        }
    }
}
