using AuthorizeTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizeTransaction.Infrastructure.Context
{
    public class AuthorizeTransactionContext : DbContext  
    {
      
        public AuthorizeTransactionContext(DbContextOptions<AuthorizeTransactionContext> options)
          : base(options)
        {
        
        }

        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Record> Records { get; set; }

       
    }
}
