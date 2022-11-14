using EmoneyService;
using Microsoft.AspNetCore.Mvc;

namespace eMoney.Test.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public WeatherForecastController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("test")]
        public async Task<ResponseOfArrayOfParameter> TestAsync()
        {
           return await _vendorService.GetInfoAsync();

        }

    }
}