using Newtonsoft.Json;

namespace AuthorizeTransaction.Domain.Entities
{
    public class Entity
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
