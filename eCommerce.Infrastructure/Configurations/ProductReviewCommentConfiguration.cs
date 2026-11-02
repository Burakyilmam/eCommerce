
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class ProductReviewCommentConfiguration
        : IEntityTypeConfiguration<ProductReviewComment>
    {
        public void Configure(EntityTypeBuilder<ProductReviewComment> builder)
        {
            builder.ToTable("ProductReviewComments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(x => x.ProductReviewId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ParentCommentId);

            // ProductReview -> ProductReviewComment
            builder.HasOne(x => x.ProductReview)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ProductReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // ParentComment -> Replies
            builder.HasOne(x => x.ParentComment)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ApplicationUser -> ProductReviewComment
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
