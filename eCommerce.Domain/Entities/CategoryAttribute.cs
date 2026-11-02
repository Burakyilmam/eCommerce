namespace eCommerce.Domain.Entities
{
    public class CategoryAttribute : BaseEntity
    {
        public int CategoryId { get; set; }

        public int ProductAttributeId { get; set; }

        public bool IsRequired { get; set; }

        public bool IsFilterable { get; set; }

        public int DisplayOrder { get; set; }

        public Category Category { get; set; } = null!;

        public ProductAttribute ProductAttribute { get; set; } = null!;
    }
}