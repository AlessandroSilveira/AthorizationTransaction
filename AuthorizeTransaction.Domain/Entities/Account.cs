using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Account : Entity
    {
        public bool ActiveCard { get; set; }
        public int AvailableLimit { get; set; }        
        public List<string> Violations { get; set; }
    }
}
