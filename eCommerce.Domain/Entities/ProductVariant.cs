
using eCommerce.Domain;

namespace eCommerce.Domain.Entities
{
    public class ProductVariant : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string? Barcode { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<ProductVariantOptionValue> OptionValues { get; set; } = new List<ProductVariantOptionValue>();

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<SellerListing> SellerListings { get; set; } = new List<SellerListing>();
    }
}
