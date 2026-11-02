namespace eCommerce.Domain.Entities
{
    public class Seller : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public string BusinessName { get; set; } = string.Empty;

        public string? TaxNumber { get; set; }

        public string? TaxOffice { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Store> Stores { get; set; } = new List<Store>();
    }
}