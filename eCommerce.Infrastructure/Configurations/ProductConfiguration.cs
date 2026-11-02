using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(500);

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.Property(x => x.IsFeatured)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.ViewCount)
                .IsRequired()
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.HasIndex(x => x.CategoryId);

            builder.HasIndex(x => x.BrandId);

            builder.HasIndex(x => new { x.IsActive, x.IsFeatured });

            // Product -> Category
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> Brand
            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}