using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Account : Entity
    {       
        public bool ActiveCard { get; set; }
        public int AvailableLimit { get; set; }


        
    }
}
