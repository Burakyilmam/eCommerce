
using eCommerce.Application.Abstractions;
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Contexts;

namespace eCommerce.Infrastructure.Repositories
{
    public class SliderRepository : GenericRepository<Slider>, ISliderRepository
    {
        public SliderRepository(AppDbContext context) : base(context)
        {

        }
    }
}
