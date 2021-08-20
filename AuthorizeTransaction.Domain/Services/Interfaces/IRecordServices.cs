using AuthorizeTransaction.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Services.Interfaces
{
    public interface IRecordServices
    {
        Task AddAsync(Record record);
        Task<List<Record>> GetAll();
        void Update(Record item);
    }
}
