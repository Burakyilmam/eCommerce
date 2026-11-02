
using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class ProductOptionValueConfiguration
        : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.ToTable("ProductOptionValues");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(100);

            // Aynı seçenek altında aynı değer tekrar edemez.
            builder.HasIndex(x => new { x.ProductOptionId, x.Value })
                .IsUnique();

            // ProductOption -> ProductOptionValue
            builder.HasOne(x => x.ProductOption)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.ProductOptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
