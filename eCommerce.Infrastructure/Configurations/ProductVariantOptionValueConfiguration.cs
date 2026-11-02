using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class ProductVariantOptionValueConfiguration
        : IEntityTypeConfiguration<ProductVariantOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductVariantOptionValue> builder)
        {
            builder.ToTable("ProductVariantOptionValues");

            builder.HasKey(x => x.Id);

            // Aynı seçenek değeri aynı varyanta iki kez eklenemez.
            builder.HasIndex(x => new
            {
                x.ProductVariantId,
                x.ProductOptionValueId
            })
                .IsUnique();

            // ProductVariant -> ProductVariantOptionValue
            builder.HasOne(x => x.ProductVariant)
                .WithMany(x => x.OptionValues)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductOptionValue -> ProductVariantOptionValue
            builder.HasOne(x => x.ProductOptionValue)
                .WithMany(x => x.VariantOptionValues)
                .HasForeignKey(x => x.ProductOptionValueId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}