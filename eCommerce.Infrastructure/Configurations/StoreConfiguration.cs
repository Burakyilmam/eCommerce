
using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.LogoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.BannerUrl)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.HasIndex(x => x.SellerId);

            // Seller -> Store
            builder.HasOne(x => x.Seller)
                .WithMany(x => x.Stores)
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
