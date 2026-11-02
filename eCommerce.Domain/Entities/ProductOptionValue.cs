namespace eCommerce.Domain.Entities
{
    public class ProductOptionValue : BaseEntity
    {
        public int ProductOptionId { get; set; }

        public ProductOption ProductOption { get; set; } = null!;

        public string Value { get; set; } = string.Empty;

        public ICollection<ProductVariantOptionValue> VariantOptionValues { get; set; } = new List<ProductVariantOptionValue>();
    }
}