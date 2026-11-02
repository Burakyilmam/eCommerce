
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Persistence.Contexts
{
    public class AppDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Infrastructure assembly içerisindeki tüm
            // IEntityTypeConfiguration sınıflarını uygular.
            builder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

        // Kullanıcı profili
        public DbSet<UserProfile> UserProfiles { get; set; }

        // Katalog
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        // Ürün varyantları ve görseller
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        // Ürün özellikleri
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }

        // Ürün seçenekleri
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        public DbSet<ProductVariantOptionValue> ProductVariantOptionValues { get; set; }

        // Ürün değerlendirmeleri
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductReviewComment> ProductReviewComments { get; set; }
        public DbSet<ProductReviewMedia> ProductReviewMedias { get; set; }
        public DbSet<ProductReviewReaction> ProductReviewReactions { get; set; }

        // Pazaryeri
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<SellerListing> SellerListings { get; set; }

        // Adres ve iletişim
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}
