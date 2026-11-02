using eCommerce.Domain.Enums;

namespace eCommerce.Domain.Entities
{
    public class ProductAttribute : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public AttributeDataType DataType { get; set; }

        public string? Unit { get; set; }

        public ICollection<ProductAttributeValue> Values { get; set; } = new List<ProductAttributeValue>();
        public ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();
    }
}