using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Transaction : Entity
    {
        public string Merchant { get; set; }
        public int Amount { get; set; }
        public DateTime Time { get; set; }        
        public List<string> Violations { get; set; }
    }
}
