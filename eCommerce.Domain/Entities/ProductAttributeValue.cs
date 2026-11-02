namespace eCommerce.Domain.Entities
{
    public class ProductAttributeValue : BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int ProductAttributeId { get; set; }

        public ProductAttribute ProductAttribute { get; set; } = null!;

        public string Value { get; set; } = string.Empty;
    }
}