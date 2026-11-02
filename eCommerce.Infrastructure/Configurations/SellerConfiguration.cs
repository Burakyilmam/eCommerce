
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Persistence.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.ToTable("Sellers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.BusinessName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TaxNumber)
                .HasMaxLength(50);

            builder.Property(x => x.TaxOffice)
                .HasMaxLength(150);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Bir kullanıcı yalnızca bir satıcı hesabına sahip olabilir.
            builder.HasIndex(x => x.UserId)
                .IsUnique();

            // Vergi numarası girilmişse benzersiz olmalı.
            builder.HasIndex(x => x.TaxNumber)
                .IsUnique()
                .HasFilter("\"TaxNumber\" IS NOT NULL");

            // ApplicationUser -> Seller
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
