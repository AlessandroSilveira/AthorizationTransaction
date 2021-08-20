using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Interfaces;

namespace AuthorizeTransaction.Domain.Services.Transactions
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IRecordRepository _recordRepository;


        public TransactionServices(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;

        }

        public Record TransactionAuthorization(Record record)
        {
           

            if (record.Account != null)
                if (!record.Account.ActiveCard)
                {                   
                    record.Account.Violations.Add(new Violations { Violation = "card-not-active" });
                    return record;
                }

            if (HighFrequency(record))
            {
                
                record.Transaction.Violations.Add(new Violations { Violation = "high-frequency-small-interval" });
                return record;
            }

            if (DoubledTransaction(record))
            {
                
                record.Transaction.Violations.Add(new Violations { Violation = "doubled-transaction" });
                return record;
            }

            VeryifyAvaliableLimitAsync(record);
            return record;
        }

        private async Task VeryifyAvaliableLimitAsync(Record record)
        {
            try
            {
                var records = await _recordRepository.GetAllAsync();
                var RecordsList = records.ToList();

                var account = RecordsList.FirstOrDefault().Account;

                if (account != null && record.Transaction != null)
                    if (account.AvailableLimit - record.Transaction.Amount < 0)                        
                        record.Transaction.Violations.Add(new Violations { Violation = "insufficient-limit" });
                    else
                    {
                        account.AvailableLimit -= record.Transaction.Amount;
                        record.Account = account;
                       await _recordRepository.UpdateAsync(record);
                    }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

            
        }

        public bool HighFrequency(Record record)
        {
            return TransactionsOnTwoMinutes(record) > 3;
        }

        public int TransactionsOnTwoMinutes(Record record)
        {
            //var minutes = record.Transaction.Time.AddMinutes(-2);

            //var teste = _recordRepository.GetAllAsync().Result;

            //var transactions = teste.Where(t => t.Transaction != null && (t.Transaction.Time >= minutes && t.Transaction.Time <= record.Transaction.Time)).Count();

            //return transactions;

            var minutes = record.Transaction.Time.AddMinutes(-2);
            var transactions = _recordRepository.GetAllAsync().Result.Where(c => c.Transaction !=null && (c.Transaction.Time >= minutes && c.Transaction.Time >= record.Transaction.Time)).Count();
            return transactions;
        }

        public bool DoubledTransaction(Record record)
        {
            var transactions = TransactionsOnTwoMinutes(record);

            if (transactions > 2)
                return _recordRepository.GetAllAsync().Result.Where(c => c.Transaction != null && ( c.Transaction.Merchant == record.Transaction.Merchant && c.Transaction.Amount == record.Transaction.Amount)).Count() > 2;

            return false;
        }
    }
}
