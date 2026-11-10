
namespace eCommerce.Domain.Entities;

public class Slider : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string? Highlight { get; set; }

    public string? Discount { get; set; }

    public string? Description { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string? ImageAlt { get; set; }

    public string? LinkUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
