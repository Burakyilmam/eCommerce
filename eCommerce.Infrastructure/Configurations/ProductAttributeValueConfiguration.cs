using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class ProductAttributeValueConfiguration
        : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.ToTable("ProductAttributeValues");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(500);

            // Aynı ürün için aynı özellik yalnızca bir kez tanımlanabilir.
            builder.HasIndex(x => new
            {
                x.ProductId,
                x.ProductAttributeId
            })
                .IsUnique();

            // Product -> ProductAttributeValue
            builder.HasOne(x => x.Product)
                .WithMany(x => x.AttributeValues)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductAttribute -> ProductAttributeValue
            builder.HasOne(x => x.ProductAttribute)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.ProductAttributeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}