using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Ouputs;
using AuthorizeTransaction.Domain.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Services.AuthorizeTransactions
{
    public class AuthorizeTransactionService
    {
        private readonly IRecordServices _recordServices;
        private readonly IAccountServices _accountServices;
        private readonly ITransactionServices _transactionServices;

        public AuthorizeTransactionService(IRecordServices recordServices, IAccountServices accountServices, ITransactionServices transactionServices)
        {
            _recordServices = recordServices;
            _accountServices = accountServices;
            _transactionServices = transactionServices;
        }

        public async Task StartReadInputTransactions()
        {
            var records = new List<Record>();      
            try
            {
                ReadInput(records);
                AuthorizeOperationsAsync(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public  void ReadInput(List<Record?> records)
        {

            Console.WriteLine("Please insert Cat operations: ");
            do
            {
                string line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                //_recordServices.AddAsync(JsonConvert.DeserializeObject<Record>(line));
                var record = JsonConvert.DeserializeObject<Record>(line);
                
                
                records.Add(record);
            } while (true);
        }

        public async Task AuthorizeOperationsAsync(List<Record> records)
        {

            //var records = await _recordServices.GetAll();

            Console.WriteLine("Authorize < operations");

            foreach (var item in records)
            {
                if (item.Transaction != null && (item.Transaction.Amount > 0 && !string.IsNullOrEmpty(item.Transaction.Merchant)))
                {
                    var response = await _transactionServices.TransactionAuthorizationAsync(item, records);


                    var outputAccount = new AccountOutput { Account = response };

                    var output = JsonConvert.SerializeObject(outputAccount);
                    PrintOutput(output);

                }
                else
                {
                   var response =  await _accountServices.AccountCreationAsync(item.Account);

                    var outputAccount = new AccountOutput { Account = response };
                    var output = JsonConvert.SerializeObject(outputAccount);

                   PrintOutput(output);
                }
            }
        }

        private static void PrintOutput(string output)
        {
            Stream stdout = Console.OpenStandardOutput();
            stdout.Write(Encoding.UTF8.GetBytes(output.ToLower()));
            stdout.Write(Encoding.UTF8.GetBytes("\n"));           
        }
    }
}



