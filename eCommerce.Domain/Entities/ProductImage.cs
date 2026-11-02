namespace eCommerce.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int? ProductVariantId { get; set; }

        public ProductVariant? ProductVariant { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string? AltText { get; set; }

        public bool IsMain { get; set; }

        public int DisplayOrder { get; set; }
    }
}