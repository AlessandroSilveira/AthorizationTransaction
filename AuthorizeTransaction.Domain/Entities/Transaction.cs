using AuthorizeTransaction.Domain.Enums;
using AuthorizeTransaction.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Transaction : Entity
    {       
        public string Merchant { get; private set; }
        public int Amount { get; private set; }
        public DateTime Time { get; private set; }

        internal void Create(Transaction transaction)
        {
            this.Amount = transaction.Amount;
            this.Id = transaction.Id;
            this.Merchant = transaction.Merchant;
            this.Time = transaction.Time;
        }

        public async Task<bool> DoubledTransactionAsync(Record record, ITransactionRepository _transactionRepository)
        {
            var transactions = await TransactionsOnTwoMinutesAsync(record, _transactionRepository);

            var teste = _transactionRepository.GetAllAsync().Result;

            if (transactions > 2)
                return _transactionRepository.GetAllAsync().Result
                  .Count(c => c.Merchant == record.Transaction.Merchant && c.Amount == record.Transaction.Amount) >= 2;

            return false;
        }

        public async Task<int> TransactionsOnTwoMinutesAsync(Record record, ITransactionRepository _transactionRepository)
        {
            var minutes = record.Transaction.Time.AddMinutes(-(int)ETransactionsIntervalMinutes.Two);
            var ListTransactions = await _transactionRepository.GetAllAsync();
            var transactions = ListTransactions.ToList();

            var countTransactions = transactions.Count(c => c != null && (c.Time >= minutes && c.Time <= record.Transaction.Time));

            return countTransactions;
        }

       

        internal async Task<bool> HighFrequencyAsync(Record record, ITransactionRepository _transactionRepository)
        {
            return await TransactionsOnTwoMinutesAsync(record, _transactionRepository) > (int)ETransactionsInMinutes.ThreeTransactions;
        }

        private async Task VeryifyAvaliableLimitAsync(Record record, Entities.Account? account, List<string> violations)
        {
            try
            {
                if (account != null && record.Transaction != null)
                    if (account.AvailableLimit - record.Transaction.Amount < 0)
                    {
                        if (!violations.Contains("insufficient-limit"))
                            violations.Add("insufficient-limit");

                        await _accountRepository.UpdateAsync(account);
                    }
                    else
                    {
                        if (violations.Contains("insufficient-limit"))
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




    }
}
