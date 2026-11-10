
using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly ISliderService _sliderService;
        private readonly IMapper _mapper;

        public SliderController(ISliderService sliderService, IMapper mapper)
        {
            _sliderService = sliderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveSliders(CancellationToken ct)
        {
            try
            {
                var sliders = await _sliderService.GetActiveSlidersAsync(ct);

                var result = _mapper.Map<List<SliderDto>>(sliders);

                return Ok(result);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                return new EmptyResult();
            }
        }
    }
}
