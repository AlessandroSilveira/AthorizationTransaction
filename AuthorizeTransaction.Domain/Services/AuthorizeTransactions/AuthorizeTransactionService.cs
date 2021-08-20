using AuthorizeTransaction.Domain.Entities;
using AuthorizeTransaction.Domain.Services.Interfaces;
using Newtonsoft.Json;

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
            try
            {
                ReadInput();
                AuthorizeOperationsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ReadInput()
        {
            
            Console.WriteLine("Please inseert Cat operations: ");
            do
            {
                string line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                _recordServices.AddAsync(JsonConvert.DeserializeObject<Record>(line));
            } while (true);
        }

        private async Task AuthorizeOperationsAsync()
        {
            var records = await _recordServices.GetAll();


            Console.WriteLine("Authorize < operations");
            foreach (var item in records)
            {
                if  ( item.Transaction != null && (item.Transaction.Amount > 0 && !string.IsNullOrEmpty(item.Transaction.Merchant)))
                   _transactionServices.TransactionAuthorization(item);
                else
                   _accountServices.AccountCreationAsync(item.Account);

                ////item.Account = Account;
                //_recordServices.Update(item);
            }

            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(records, Formatting.Indented));


        }

    }
}
