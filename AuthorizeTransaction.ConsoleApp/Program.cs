using AuthorizeTransaction.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AuthorizeTransaction.ConsoleApp.DependencyInjection;
using AuthorizeTransaction.Domain.Services.AuthorizeTransactions;

namespace AuthorizeTransaction.ConsoleApp
{

    public class Program
    {
        protected Program(IConfiguration configuration) => Configuration = configuration;

        public static IConfiguration Configuration { get; set; }
        
        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<AuthorizeTransactionService>().StartReadInputTransactions().Wait();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = GetConfiguration();
            serviceCollection
            .AddSingleton(typeof(IConfiguration), configuration)
            .AddDbContext<AuthorizeTransactionContext>(opt => opt.UseInMemoryDatabase("AuthorizeTransactions"))           
            .AddAuthorizeTransactionsConfigurations()
          
            .AddSingleton<AuthorizeTransactionService>();

           // var optionsBuilder = new DbContextOptionsBuilder<AuthorizeTransactionContext>();
           // optionsBuilder.UseInMemoryDatabase("AuthorizeTransactions");

           //var  _context = new AuthorizeTransactionContext(optionsBuilder.Options);
           // //_context.Database.EnsureDeleted();
        }
        private static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true);
            return configBuilder.Build();
        }

    }
}