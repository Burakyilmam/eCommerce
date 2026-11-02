namespace eCommerce.Domain.Entities
{
    public class ProductVariantOptionValue : BaseEntity
    {
        public int ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; } = null!;

        public int ProductOptionValueId { get; set; }

        public ProductOptionValue ProductOptionValue { get; set; } = null!;
    }
}