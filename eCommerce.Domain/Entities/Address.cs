using eCommerce.Domain.Enums;

namespace eCommerce.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string? UserId { get; set; }

        public int? SellerId { get; set; }

        public int? StoreId { get; set; }

        public string Title { get; set; } = string.Empty;

        public AddressType AddressType { get; set; }

        public AddressUsageType AddressUsage { get; set; }

        public string Country { get; set; } = "Türkiye";

        public string City { get; set; } = string.Empty;

        public string? District { get; set; }

        public string? Neighborhood { get; set; }

        public string? Street { get; set; }

        public string? BuildingNumber { get; set; }

        public string? ApartmentNumber { get; set; }

        public string? PostalCode { get; set; }

        public string AddressLine { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }
}