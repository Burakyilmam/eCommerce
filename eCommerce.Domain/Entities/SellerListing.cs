namespace eCommerce.Domain.Entities
{
    public class SellerListing : BaseEntity
    {
        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        public string SKU { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal? CompareAtPrice { get; set; }

        public int StockQuantity { get; set; }

        public string CurrencyCode { get; set; } = "TRY";

        public bool IsActive { get; set; } = true;
    }
}
