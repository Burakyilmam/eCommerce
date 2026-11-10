
using eCommerce.Application.Abstractions;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Implementations
{
    public class SliderService : GenericService<Slider>, ISliderService
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderService(ISliderRepository sliderRepository, IUnitOfWork unitOfWork) : base(sliderRepository, unitOfWork)
        {
            _sliderRepository = sliderRepository;
        }

        public async Task<IReadOnlyList<Slider>> GetActiveSlidersAsync(CancellationToken ct = default)
        {
            var sliders = await _sliderRepository.GetAllAsync(ct);
            var now = DateTime.UtcNow;

            return sliders
                .Where(x => x.IsActive
                    && (!x.StartDate.HasValue || x.StartDate.Value <= now)
                    && (!x.EndDate.HasValue || x.EndDate.Value >= now))
                .OrderBy(x => x.DisplayOrder)
                .ToList();
        }
    }
}
