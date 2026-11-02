namespace eCommerce.Domain.Entities
{
    public class Store : BaseEntity
    {
        public int SellerId { get; set; }
        public Seller Seller { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? LogoUrl { get; set; }

        public string? BannerUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<SellerListing> Listings { get; set; } = new List<SellerListing>();
    }
}
