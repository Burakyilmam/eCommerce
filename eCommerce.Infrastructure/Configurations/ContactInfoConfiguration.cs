
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
    {
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_ContactInfos_ExactlyOneOwner",
                    """
        (CASE WHEN "UserId" IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN "SellerId" IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN "StoreId" IS NOT NULL THEN 1 ELSE 0 END) = 1
        """));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .HasMaxLength(450);

            builder.Property(x => x.ContactType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            // Normal indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.SellerId);
            builder.HasIndex(x => x.StoreId);
            builder.HasIndex(x => new { x.ContactType, x.Value });

            // Kullanıcı: iletişim türü başına tek bir birincil kayıt.
            builder.HasIndex(x => new { x.UserId, x.ContactType })
                .IsUnique()
                .HasFilter("\"UserId\" IS NOT NULL AND \"IsPrimary\" = TRUE");

            builder.HasIndex(x => new { x.SellerId, x.ContactType })
                .IsUnique()
                .HasFilter("\"SellerId\" IS NOT NULL AND \"IsPrimary\" = TRUE");

            builder.HasIndex(x => new { x.StoreId, x.ContactType })
                .IsUnique()
                .HasFilter("\"StoreId\" IS NOT NULL AND \"IsPrimary\" = TRUE");

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
