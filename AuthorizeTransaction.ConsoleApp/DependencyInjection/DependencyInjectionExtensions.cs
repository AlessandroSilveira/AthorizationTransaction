using AuthorizeTransaction.Domain.Repositories.Interfaces;
using AuthorizeTransaction.Domain.Repositories.Interfaces.Base;
using AuthorizeTransaction.Domain.Services.Account;
using AuthorizeTransaction.Domain.Services.Interfaces;
using AuthorizeTransaction.Domain.Services.Records;
using AuthorizeTransaction.Domain.Services.Transactions;
using AuthorizeTransaction.Infrastructure.Context;
using AuthorizeTransaction.Infrastructure.Repositories;
using AuthorizeTransaction.Infrastructure.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizeTransaction.ConsoleApp.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAuthorizeTransactionsConfigurations(this IServiceCollection services) =>
            services
            //Services
            .AddTransient<IRecordServices, RecordServices>()
            .AddTransient<ITransactionServices, TransactionServices>()
            .AddTransient<IAccountServices, AccountServices>()

            //Repositories
            .AddTransient<IRecordRepository, RecordRepository>()            
            .AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))

            //Context
            .AddScoped<AuthorizeTransactionContext>()
            ;


    }
}
