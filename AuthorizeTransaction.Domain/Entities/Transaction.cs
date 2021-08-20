using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Transaction : Entity
    {
        public Transaction()
        {
            this.Violations = new List<Violations>();
        }
        public string Merchant { get; set; }
        public int Amount { get; set; }
        public DateTime Time { get; set; }

        public List<Violations> Violations { get; set; }
    }
}
