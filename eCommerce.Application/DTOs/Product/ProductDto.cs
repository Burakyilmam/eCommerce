
namespace eCommerce.Application.DTOs.Products;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? ShortDescription { get; set; }

    public int CategoryId { get; set; }
    public int? BrandId { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }

    // Satıcı ve varyant bilgileri
    public int? SellerListingId { get; set; }
    public int? ProductVariantId { get; set; }

    // Fiyat bilgileri
    public decimal? Price { get; set; }
    public decimal? OldPrice { get; set; }

    // Stok ve para birimi
    public int? StockQuantity { get; set; }
    public string? CurrencyCode { get; set; }

    // Ana ürün görseli
    public string? Image { get; set; }
}
