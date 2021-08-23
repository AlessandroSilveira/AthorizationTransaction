using System.Collections.Generic;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Violation
    {
        public Violation()
        {
            Violations = new List<string>();
        }
        public List<string> Violations { get; set; }
    }
}