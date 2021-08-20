using AuthorizeTransaction.Domain.Entities;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface IRecordServices
    {
        Task AddAsync(Record record);
        Task<List<Record>> GetAll();
        void Update(Record item);
    }
}
