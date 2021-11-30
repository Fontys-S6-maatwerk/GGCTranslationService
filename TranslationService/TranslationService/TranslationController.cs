using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationService.Mappers;
using TranslationService.Services.Services;

namespace TranslationService
{
    [ApiController]
    [Route("translations")]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly ITranslationService _service;

        public TranslationController(ILogger<TranslationController> logger, ITranslationService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet]
        [Route("{solutionId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTranslationsForSolution([FromRoute] Guid solutionId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(TranslationMapper.TranslationToTranslationDto(await _service.GetTranslations(solutionId)));
        }

        [HttpGet]
        [Route("{solutionId}/{locale}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTranslationForSolution([FromRoute] Guid solutionId, [FromRoute] string locale)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            return Ok(TranslationMapper.TranslationToTranslationDto(await _service.GetTranslation(solutionId, locale)));
        }
    }
}