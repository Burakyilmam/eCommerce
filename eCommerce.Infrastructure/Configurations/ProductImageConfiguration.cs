
using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class ProductImageConfiguration
        : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.AltText)
                .HasMaxLength(250);

            builder.Property(x => x.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.ProductVariantId);

            // Product -> ProductImage
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductVariant -> ProductImage
            builder.HasOne(x => x.ProductVariant)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
