
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class ProductReviewConfiguration
        : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_ProductReviews_Rating",
                    "\"Rating\" >= 1 AND \"Rating\" <= 5"));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.Title)
                .HasMaxLength(200);

            builder.Property(x => x.Comment)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.IsVerifiedPurchase)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => new
            {
                x.ProductId,
                x.IsApproved,
                x.IsDeleted
            });

            // Product -> ProductReview
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser -> ProductReview
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
