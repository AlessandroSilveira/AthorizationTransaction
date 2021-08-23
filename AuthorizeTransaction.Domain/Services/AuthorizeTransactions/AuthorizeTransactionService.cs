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
        private readonly IAccountServices _accountServices;
        private readonly ITransactionServices _transactionServices;

        public AuthorizeTransactionService(IAccountServices accountServices, ITransactionServices transactionServices)
        {          
            _accountServices = accountServices;
            _transactionServices = transactionServices;
        }

        public async Task StartReadInputTransactions()
        {
            var records = new List<Record>();      
            try
            {
                ReadInput(records);
                await AuthorizeOperationsAsync(records);
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

                records.Add(JsonConvert.DeserializeObject<Record>(line));
            } while (true);
        }

        public async Task AuthorizeOperationsAsync(List<Record> records)
        {
            var violations = new Violation();

            Console.WriteLine("Authorize < operations");

            foreach (var item in records)
            {
                if (item.Transaction != null && (item.Transaction.Amount > 0 && !string.IsNullOrEmpty(item.Transaction.Merchant)))
                {
                    var response = await _transactionServices.TransactionAuthorizationAsync(item,  violations.Violations);

                    PrintOutput(violations, response);
                }
                else
                {
                   var response =  await _accountServices.AccountCreationAsync(item.Account, violations.Violations);

                    PrintOutput(violations, response);
                }
            }
        }


        private static void PrintOutput(Violation violations, Entities.Account response)
        {
            var outputAccount = new AccountOutput { Account = response, Violations = violations.Violations };

            var output = JsonConvert.SerializeObject(outputAccount);
            Stream stdout = Console.OpenStandardOutput();
            stdout.Write(Encoding.UTF8.GetBytes(output.ToLower()));
            stdout.Write(Encoding.UTF8.GetBytes("\n"));
            
        }
    }
}



