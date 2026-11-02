using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class ProductReviewMediaConfiguration
        : IEntityTypeConfiguration<ProductReviewMedia>
    {
        public void Configure(EntityTypeBuilder<ProductReviewMedia> builder)
        {
            builder.ToTable("ProductReviewMedias");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MediaUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.MediaType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.AltText)
                .HasMaxLength(250);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Index
            builder.HasIndex(x => x.ProductReviewId);

            // ProductReview -> Medias
            builder.HasOne(x => x.ProductReview)
                .WithMany(x => x.Medias)
                .HasForeignKey(x => x.ProductReviewId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}