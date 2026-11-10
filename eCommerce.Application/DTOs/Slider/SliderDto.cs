
namespace eCommerce.Application.DTOs
{
    public class SliderDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Highlight { get; set; }
        public string? Discount { get; set; }
        public string? Description { get; set; }

        public string Image { get; set; } = string.Empty;
        public string? ImageAlt { get; set; }
        public string? LinkUrl { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
