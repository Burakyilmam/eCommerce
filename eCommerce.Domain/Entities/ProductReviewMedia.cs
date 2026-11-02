using eCommerce.Domain.Enums;

namespace eCommerce.Domain.Entities
{
    public class ProductReviewMedia : BaseEntity
    {
        public int ProductReviewId { get; set; }

        public string MediaUrl { get; set; } = string.Empty;

        public MediaType MediaType { get; set; }

        public string? AltText { get; set; }

        public int DisplayOrder { get; set; }

        public ProductReview ProductReview { get; set; } = null!;
    }
}