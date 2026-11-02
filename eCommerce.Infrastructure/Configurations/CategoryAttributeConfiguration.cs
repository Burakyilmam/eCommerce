using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class CategoryAttributeConfiguration
        : IEntityTypeConfiguration<CategoryAttribute>
    {
        public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
        {
            builder.ToTable("CategoryAttributes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsFilterable)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Aynı özellik aynı kategoriye iki kez eklenemez.
            builder.HasIndex(x => new
            {
                x.CategoryId,
                x.ProductAttributeId
            })
                .IsUnique();

            // Category -> CategoryAttribute
            builder.HasOne(x => x.Category)
                .WithMany(x => x.CategoryAttributes)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductAttribute -> CategoryAttribute
            builder.HasOne(x => x.ProductAttribute)
                .WithMany(x => x.CategoryAttributes)
                .HasForeignKey(x => x.ProductAttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}