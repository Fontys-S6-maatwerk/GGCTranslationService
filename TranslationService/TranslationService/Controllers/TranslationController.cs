using System;
using System.Threading.Tasks;
using DeepL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TranslationService.MQ;
using TranslationService.MQ.Send;

namespace TranslationService.Controllers
{
    [ApiController]
    [Route("/translate")]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly IConfiguration _config;

        private readonly ITranslationSender _translationSender;
        
        public TranslationController(ILogger<TranslationController> logger, IConfiguration config, ITranslationSender translationSender)
        {
            _config = config;
            _translationSender = translationSender;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Translate([FromBody] string x)
        {
            return Ok();
        }
    }
}
