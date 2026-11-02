using eCommerce.Domain;
using eCommerce.Domain.Enums;

namespace eCommerce.Domain.Entities
{
    public class ProductReviewReaction : BaseEntity
    {
        public int? ProductReviewId { get; set; }

        public int? ProductReviewCommentId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ReactionType ReactionType { get; set; }

        public ProductReview? ProductReview { get; set; }

        public ProductReviewComment? ProductReviewComment { get; set; }
    }
}