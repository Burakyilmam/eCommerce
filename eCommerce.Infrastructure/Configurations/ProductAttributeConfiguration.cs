using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttributes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Unit)
                .HasMaxLength(50);

            // Attribute kodu benzersiz olmalı
            builder.HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}