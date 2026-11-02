namespace eCommerce.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Gender { get; set; }
    }
}