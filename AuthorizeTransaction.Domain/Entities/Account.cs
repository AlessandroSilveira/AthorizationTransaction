using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Account : Entity
    {
        //public Account()
        //{
        //    this.Violations = new List<string>();
        //}
        public bool ActiveCard { get; set; }
        public int AvailableLimit { get; set; }        
        //[NotMapped]
        //public List<string> Violations { get; set; }
    }
}
