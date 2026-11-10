
using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Mappings
{
    public class SliderMappingProfile : Profile
    {
        public SliderMappingProfile()
        {
            CreateMap<Slider, SliderDto>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}
