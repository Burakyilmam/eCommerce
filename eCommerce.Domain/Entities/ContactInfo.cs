using eCommerce.Domain.Enums;

namespace eCommerce.Domain.Entities
{
    public class ContactInfo : BaseEntity
    {
        public string? UserId { get; set; }

        public int? SellerId { get; set; }

        public int? StoreId { get; set; }

        public ContactType ContactType { get; set; }

        public string Value { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public bool IsVerified { get; set; }
    }
}