using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<attachmentOptions> _attachmentOptions;

        public ConfigController(IConfiguration configuration, IOptions<attachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                ConnectionStrings = _configuration["ConnectionStrings:DefautConnection"],
                attachmentOptions = _attachmentOptions.Value
            };

            return Ok(config);
        }
    }
}
