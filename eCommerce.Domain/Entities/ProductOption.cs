namespace eCommerce.Domain.Entities
{
    public class ProductOption : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public ICollection<ProductOptionValue> Values { get; set; } = new List<ProductOptionValue>();
    }
}