
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
