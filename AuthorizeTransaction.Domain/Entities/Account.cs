using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Account : Entity
    {
        public Account()
        {
            this.Violations = new List<Violations>();   
        }
        public bool ActiveCard { get; set; }
        public int AvailableLimit { get; set; }        
        public List<Violations> Violations { get; set; }
    }
}
