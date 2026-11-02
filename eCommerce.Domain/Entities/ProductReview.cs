namespace eCommerce.Domain.Entities
{
    public class ProductReview : BaseEntity
    {
        public int ProductId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string? Title { get; set; }

        public string Comment { get; set; } = string.Empty;

        public int Rating { get; set; }

        public bool IsVerifiedPurchase { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public Product Product { get; set; } = null!;

        public ICollection<ProductReviewComment> Comments { get; set; } = new List<ProductReviewComment>();
        public ICollection<ProductReviewMedia> Medias { get; set; } = new List<ProductReviewMedia>();
        public ICollection<ProductReviewReaction> Reactions { get; set; } = new List<ProductReviewReaction>();
    }
}