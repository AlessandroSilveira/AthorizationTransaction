namespace AuthorizeTransaction.Domain.Entities
{
    public class Record : Entity
    {
        public Account Account { get; set; }
        public Transaction Transaction { get; set; }
        
    }
}
