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

        [HttpPost("Pay")]
        public async Task<ResponseOfTransaction> PayAsync()
        {
            return await _vendorService.PayAsync();
        }

        [HttpPost("GetTransactionInfo")]
        public async Task<ResponseOfTransaction> GetTransactionInfoAsync()
        {
            var payResponse = await _vendorService.PayAsync();
            var transactionCode = payResponse.Value.Code;
            return await _vendorService.GetTransactionInfoAsync(transactionCode.ToString());
        }

    }
}