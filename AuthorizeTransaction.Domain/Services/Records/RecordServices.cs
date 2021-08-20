using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Interfaces;

namespace AuthorizeTransaction.Domain.Services.Records
{
    public class RecordServices : IRecordServices
    {
        private readonly IRecordRepository _recordRepository;

        public RecordServices(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task AddAsync(Record record)
        {
            try
            {

                await _recordRepository.AddAsync(record);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<List<Record>> GetAll()
        {
            IEnumerable<Record> record = await _recordRepository.GetAllAsync();

            List<Record> RecordsList = record.ToList();
            return RecordsList;
        }

        public void Update(Record item)
        {
            _recordRepository.UpdateAsync(item);
        }
    }
}
