using System.Collections.Generic;

namespace AuthorizeTransaction.Domain.Ouputs
{
    public class AccountOutput
    {
        public Entities.Account Account { get; set; }
        public List<string> Violations { get; set; }
    };
}



