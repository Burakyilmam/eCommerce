using eCommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerce.Infrastructure.Configurations
{
    public class SellerListingConfiguration
        : IEntityTypeConfiguration<SellerListing>
    {
        public void Configure(EntityTypeBuilder<SellerListing> builder)
        {
            builder.ToTable("SellerListings", table =>
            {
                table.HasCheckConstraint(
                    "CK_SellerListings_Price",
                    "\"Price\" >= 0");

                table.HasCheckConstraint(
                    "CK_SellerListings_CompareAtPrice",
                    "\"CompareAtPrice\" IS NULL OR \"CompareAtPrice\" >= 0");

                table.HasCheckConstraint(
                    "CK_SellerListings_StockQuantity",
                    "\"StockQuantity\" >= 0");
            });

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SKU)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.CompareAtPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.StockQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3)
                .HasDefaultValue("TRY")
                .IsFixedLength();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.StoreId);

            builder.HasIndex(x => x.ProductVariantId);

            // SKU mağaza içerisinde benzersiz olmalı.
            builder.HasIndex(x => new
            {
                x.StoreId,
                x.SKU
            })
                .IsUnique();

            // Store -> SellerListing
            builder.HasOne(x => x.Store)
                .WithMany(x => x.Listings)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductVariant -> SellerListing
            builder.HasOne(x => x.ProductVariant)
                .WithMany(x => x.SellerListings)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}