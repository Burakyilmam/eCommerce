
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Interfaces
{
    public interface ISliderService : IGenericService<Slider>
    {
        Task<IReadOnlyList<Slider>> GetActiveSlidersAsync(CancellationToken ct = default);
    }
}
