
using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class ProductVariantConfiguration
        : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Barcode)
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => x.Barcode)
                .IsUnique()
                .HasFilter("\"Barcode\" IS NOT NULL");

            // Product -> ProductVariant
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Variants)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductVariant -> ProductVariantOptionValue
            builder.HasMany(x => x.OptionValues)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // SellerListing ilişkisi
            builder.HasMany(x => x.SellerListings)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
