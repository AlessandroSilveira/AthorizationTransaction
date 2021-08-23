using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Ouputs;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<Entities.Account> TransactionAuthorizationAsync(Record record, List<Record> records, List<string> violations)
        {   
            var accountStored = await _accountRepository.GetAllAsync();
            var account = accountStored.ToList().FirstOrDefault();
            
            if (account == null)
            {
                violations.Add("account-not-initialized");
                return new Entities.Account
                { 
                };            
            }

            if (account != null && !account.ActiveCard)
            {
                if(!violations.Contains("card-not-active"))
                    violations.Add("card-not-active");

                await _accountRepository.UpdateAsync(account);
                return account;
            }

            if (await HighFrequencyAsync(record, records))
                violations.Add("high-frequency-small-interval");

            if (await DoubledTransactionAsync(record, records))
                violations.Add("doubled-transaction");

            await VeryifyAvaliableLimitAsync(record, account, violations);

            return account;
        }


        private async Task VeryifyAvaliableLimitAsync(Record record, Entities.Account? account, List<string> violations)
        {
            try
            {
                var records = await _recordRepository.GetAllAsync();
                var RecordsList = records.ToList();
               

                if (account != null && record.Transaction != null)
                    if (account.AvailableLimit - record.Transaction.Amount < 0)
                    {
                        violations.Add("insufficient-limit");
                        await _accountRepository.UpdateAsync(account);
                    }
                    else
                    {
                        
                        if(violations.Contains("insufficient-limit"))
                            violations.Remove("insufficient-limit");

                        account.AvailableLimit -= record.Transaction.Amount;
                        
                        await _accountRepository.UpdateAsync(account);
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<bool> HighFrequencyAsync(Record record, List<Record> records)
        {
            return await TransactionsOnTwoMinutesAsync(record, records) > 3;
        }

        public async Task<int> TransactionsOnTwoMinutesAsync(Record record, List<Record> records)
        {
            var minutes = record.Transaction.Time.AddMinutes(-2);
            var transactions = await _transactionRepository.GetAllAsync();
            var list = transactions.ToList();

            var countTransactions = records.Count(c => c.Transaction != null && (c.Transaction.Time >= minutes && c.Transaction.Time <= record.Transaction.Time));
            return countTransactions;
        }

        public async Task<bool> DoubledTransactionAsync(Record record, List<Record> records)
        {
            var transactions = await TransactionsOnTwoMinutesAsync(record, records);

            if (transactions > 2)
                return _recordRepository.GetAllAsync().Result.Any(c => c.Transaction != null && (c.Transaction.Merchant == record.Transaction.Merchant && c.Transaction.Amount == record.Transaction.Amount));

            return false;
        }

      
    }
}
