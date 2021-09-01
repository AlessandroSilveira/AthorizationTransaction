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

        public void Create(int id, int amount, string merchant, DateTime time)
        {
            this.Amount = amount;
            this.Id = id;
            this.Merchant = merchant;
            this.Time =time;
        }

       
    }
}
