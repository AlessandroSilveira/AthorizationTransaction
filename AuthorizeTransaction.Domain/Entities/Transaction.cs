using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Transaction : Entity
    {
        //public Transaction()
        //{
        //    this.Violations = new List<string>();
        //}
        public string Merchant { get; set; }
        public int Amount { get; set; }
        public DateTime Time { get; set; }

        //[NotMapped]
        //public List<string> Violations { get; set; }
    }
}
