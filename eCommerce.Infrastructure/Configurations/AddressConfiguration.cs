
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(t =>
              t.HasCheckConstraint(
                  "CK_Addresses_ExactlyOneOwner",
                  """
        (CASE WHEN "UserId" IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN "SellerId" IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN "StoreId" IS NOT NULL THEN 1 ELSE 0 END) = 1
        """));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .HasMaxLength(450);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AddressType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.AddressUsage)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("Türkiye");

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.District)
                .HasMaxLength(100);

            builder.Property(x => x.Neighborhood)
                .HasMaxLength(150);

            builder.Property(x => x.Street)
                .HasMaxLength(200);

            builder.Property(x => x.BuildingNumber)
                .HasMaxLength(30);

            builder.Property(x => x.ApartmentNumber)
                .HasMaxLength(30);

            builder.Property(x => x.PostalCode)
                .HasMaxLength(20);

            builder.Property(x => x.AddressLine)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);

            // Normal indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.SellerId);
            builder.HasIndex(x => x.StoreId);
            builder.HasIndex(x => new { x.City, x.District });

            // Kullanıcı başına yalnızca bir varsayılan adres.
            builder.HasIndex(x => x.UserId)
                .IsUnique()
                .HasFilter("\"UserId\" IS NOT NULL AND \"IsDefault\" = TRUE");

            builder.HasIndex(x => x.SellerId)
                .IsUnique()
                .HasFilter("\"SellerId\" IS NOT NULL AND \"IsDefault\" = TRUE");

            builder.HasIndex(x => x.StoreId)
                .IsUnique()
                .HasFilter("\"StoreId\" IS NOT NULL AND \"IsDefault\" = TRUE");

            // User relationship
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seller relationship
            builder.HasOne<Seller>()
                .WithMany()
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Store relationship
            builder.HasOne<Store>()
                .WithMany()
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
