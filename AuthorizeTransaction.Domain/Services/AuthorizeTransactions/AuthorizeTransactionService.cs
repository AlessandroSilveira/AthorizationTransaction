using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Ouputs;
using AuthorizeTransaction.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IConfiguration _configuration;
       
        List<Record> records = new();

        public AuthorizeTransactionService(IAccountServices accountServices, ITransactionServices transactionServices, IConfiguration configuration)
        {
            _accountServices = accountServices;
            _transactionServices = transactionServices;
            _configuration = configuration;
        }

        public async Task StartReadInputTransactions(FileSystemWatcher watcher)
        {
            watcher.Created += new FileSystemEventHandler(ReadInput);
            watcher.Changed += new FileSystemEventHandler(ReadInput);
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public void  ReadInput(object source, FileSystemEventArgs e)
        { 
            var enumLines = File.ReadLines( _configuration.GetSection("File:Path").ToString() + e.Name, Encoding.UTF8);
            
            foreach (var line in enumLines)
            {
                var linha = line;
                if (string.IsNullOrEmpty(line))
                        break;

                linha = linha.Replace("active-card", "activecard");
                linha = linha.Replace("available-limit", "availablelimit");
                try
                {
                    records.Add(JsonConvert.DeserializeObject<Record>(linha));
                }
                catch
                {
                    continue;
                }
                
            }
            AuthorizeOperationsAsync(records).Wait();
        }

        public async Task AuthorizeOperationsAsync(List<Record> records)
        {
            var violations = new Violation();
            var response = new Entities.Account();

            foreach (var item in records)
            {
                if (item.Transaction != null && (item.Transaction.Amount > 0 && !string.IsNullOrEmpty(item.Transaction.Merchant)))
                {
                     response = await _transactionServices.TransactionAuthorizationAsync(item,  violations.Violations);
                    PrintOutput(violations, response);
                }
                else
                {
                    response =  await _accountServices.AccountCreationAsync(item.Account, violations.Violations);
                    PrintOutput(violations, response);
                }
            }
            
        }


        private  void PrintOutput(Violation violations, Entities.Account response)
        {
            string folder = _configuration.GetSection("File:Path").ToString();
            string arquivo = Path.Combine(_configuration.GetSection("File:Path").ToString(), _configuration.GetSection("File:FileName").ToString());
            var outputAccount = new AccountOutput { Account = response, Violations = violations.Violations };
            var output = JsonConvert.SerializeObject(outputAccount);
           
            if (!File.Exists(arquivo))
            {
                using StreamWriter sw = new(arquivo, false);
                if (new FileInfo(arquivo).Length == 0)
                {
                    sw.WriteLine("$ authorize < operations");
                    sw.WriteLine("\n");
                }   
                
                sw.WriteLine(output.ToLower());
                sw.Flush();
                sw.Close();

            }
            else
            {
                using StreamWriter sw = File.AppendText(arquivo);
                if (new FileInfo(arquivo).Length == 0)
                {
                    sw.WriteLine("$ authorize < operations");
                    sw.WriteLine("\n");
                }

                sw.WriteLine(output.ToLower());
                sw.Flush();
                sw.Close();

            }
        }
    }
}



