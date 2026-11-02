namespace eCommerce.Domain.Entities
{
    public class ProductReviewComment : BaseEntity
    {
        public int ProductReviewId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public ProductReview ProductReview { get; set; } = null!;

        public ProductReviewComment? ParentComment { get; set; }

        public ICollection<ProductReviewComment> Replies { get; set; } = new List<ProductReviewComment>();

        public ICollection<ProductReviewReaction> Reactions { get; set; } = new List<ProductReviewReaction>();
    }
}