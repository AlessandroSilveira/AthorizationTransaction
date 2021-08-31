using AuthorizeTransaction.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AuthorizeTransaction.ConsoleApp.DependencyInjection;
using AuthorizeTransaction.Domain.Services.AuthorizeTransactions;
using System.IO;

namespace AuthorizeTransaction.ConsoleApp
{

    public class Program
    {
        protected Program(IConfiguration configuration) => Configuration = configuration;

        public static IConfiguration Configuration { get; set; }
        
        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var configuration = GetConfiguration();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<AuthorizeTransactionContext>();

            context.Database.EnsureDeleted();
            const string PATH = @"C:\Users\alessandro.silveira\source\repos\AthorizationTransaction\transactions\";
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                //string filePath = PATH;
                Path = PATH,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.*"
            };

            serviceProvider.GetService<AuthorizeTransactionService>().StartReadInputTransactions(watcher, configuration).Wait();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
           
            serviceCollection
            .AddSingleton(typeof(IConfiguration), configuration)
            .AddDbContext<AuthorizeTransactionContext>(opt => opt.UseInMemoryDatabase("AuthorizeTransactions"))
            .AddAuthorizeTransactionsConfigurations()
            .AddSingleton<AuthorizeTransactionService>();
            
        }
        private static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true);
            return configBuilder.Build();
        }

    }
}