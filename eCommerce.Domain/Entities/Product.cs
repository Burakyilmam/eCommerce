namespace eCommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int? BrandId { get; set; }

        public Brand? Brand { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

        public ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();

        public ICollection<ProductOption> Options { get; set; } = new List<ProductOption>();

        public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    }
}