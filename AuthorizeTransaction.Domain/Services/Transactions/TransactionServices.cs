﻿using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Enums;
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
        
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionServices( ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
           
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Entities.Account> TransactionAuthorizationAsync(Record record, List<string> violations)
        {
            await _transactionRepository.AddAsync(record.Transaction);
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

            if (await HighFrequencyAsync(record))
                violations.Add("high-frequency-small-interval");

            if (await DoubledTransactionAsync(record))
                violations.Add("doubled-transaction");

            await VeryifyAvaliableLimitAsync(record, account, violations);

            return account;
        }


        private async Task VeryifyAvaliableLimitAsync(Record record, Entities.Account? account, List<string> violations)
        {
            try
            {
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

        public async Task<bool> HighFrequencyAsync(Record record)
        {
            return await TransactionsOnTwoMinutesAsync(record) > (int)ETransactionsInMinutes.ThreeTransactions;
        }

        public async Task<int> TransactionsOnTwoMinutesAsync(Record record)
        {
            var minutes = record.Transaction.Time.AddMinutes(-(int)ETransactionsIntervalMinutes.Two);
            var ListTransactions = await _transactionRepository.GetAllAsync();
            var transactions = ListTransactions.ToList();

            var countTransactions = transactions.Count(c => c != null && (c.Time >= minutes && c.Time <= record.Transaction.Time));
            return countTransactions;
        }

        public async Task<bool> DoubledTransactionAsync(Record record)
        {
            var transactions = await TransactionsOnTwoMinutesAsync(record);

            if (transactions > 2)
                return _transactionRepository.GetAllAsync().Result.Any(c => c != null && (c.Merchant == record.Transaction.Merchant && c.Amount == record.Transaction.Amount));

            return false;
        }

      
    }
}
