
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class ProductReviewReactionConfiguration
        : IEntityTypeConfiguration<ProductReviewReaction>
    {
        public void Configure(EntityTypeBuilder<ProductReviewReaction> builder)
        {
            builder.ToTable("ProductReviewReactions", table =>
            {
                table.HasCheckConstraint(
                    "CK_ProductReviewReactions_ExactlyOneTarget",
                    "(\"ProductReviewId\" IS NOT NULL AND \"ProductReviewCommentId\" IS NULL) OR " +
                    "(\"ProductReviewId\" IS NULL AND \"ProductReviewCommentId\" IS NOT NULL)");
            });

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.ReactionType)
                .IsRequired()
                .HasConversion<int>();

            // Kullanıcı başına değerlendirmeye tek tepki.
            builder.HasIndex(x => new
            {
                x.ProductReviewId,
                x.UserId
            })
 .IsUnique()
 .HasFilter("\"ProductReviewId\" IS NOT NULL");

            builder.HasIndex(x => new
            {
                x.ProductReviewCommentId,
                x.UserId
            })
            .IsUnique()
            .HasFilter("\"ProductReviewCommentId\" IS NOT NULL");

            // ProductReview -> ProductReviewReaction
            builder.HasOne(x => x.ProductReview)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.ProductReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductReviewComment -> ProductReviewReaction
            builder.HasOne(x => x.ProductReviewComment)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.ProductReviewCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ApplicationUser -> ProductReviewReaction
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
