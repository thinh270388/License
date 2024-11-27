namespace License.Models.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        //[JsonIgnore]
        //public List<Product>? Products { get; set; }
    }
}
