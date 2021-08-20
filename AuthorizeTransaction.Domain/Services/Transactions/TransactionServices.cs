using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Services.Transactions
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IRecordRepository _recordRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;


        public TransactionServices(IRecordRepository recordRepository, ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _recordRepository = recordRepository;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Record> TransactionAuthorizationAsync(Record record)
        {
            
            var transactionStored = await _transactionRepository.GetByIdAsync(record.Transaction.Id);
            var accountStored = await _accountRepository.GetAllAsync();
            var account = accountStored.ToList().FirstOrDefault();

            if (account != null)
                if (!account.ActiveCard)
                {  
                    account.Violations.Add("card-not-active");
                    await _accountRepository.UpdateAsync(account);

                    record.Account = account;
                    return record;
                }

            if (HighFrequency(record))               
                record.Transaction.Violations.Add("high-frequency-small-interval");               

            if (DoubledTransaction(record))
                record.Transaction.Violations.Add("doubled-transaction");

            await VeryifyAvaliableLimitAsync(record);

            await _transactionRepository.UpdateAsync(transactionStored);

            return record;
        }



        private async Task VeryifyAvaliableLimitAsync(Record record)
        {
            try
            {
                var records = await _recordRepository.GetAllAsync();
                var RecordsList = records.ToList();
                var account = RecordsList.FirstOrDefault()?.Account;

                if (account != null && record.Transaction != null)
                    if (account.AvailableLimit - record.Transaction.Amount < 0)
                        account.Violations.Add("insufficient-limit");
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
            var minutes = record.Transaction.Time.AddMinutes(-2);
            var transactions = _recordRepository.GetAllAsync().Result.Where(c => c.Transaction != null && (c.Transaction.Time >= minutes && c.Transaction.Time >= record.Transaction.Time)).Count();
            return transactions;
        }

        public bool DoubledTransaction(Record record)
        {
            var transactions = TransactionsOnTwoMinutes(record);

            if (transactions > 2)
                return _recordRepository.GetAllAsync().Result.Where(c => c.Transaction != null && (c.Transaction.Merchant == record.Transaction.Merchant && c.Transaction.Amount == record.Transaction.Amount)).Count() > 2;

            return false;
        }
    }
}
