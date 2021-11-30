using System;
using System.Threading.Tasks;
using DeepL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TranslationService.Services.Mappers;
using Translation = TranslationService.Dal.Models.Translation;

namespace TranslationService.Services.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TranslationService> _logger;

        public TranslationService(IConfiguration config, ILogger<TranslationService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            var key = _config.GetSection("DeepL")["key"];

            Translation t = null;
            
            using (var client = new DeepLClient(key, useFreeApi: true))
            {
                try
                {
                    _logger.LogInformation("Locale found: {request.Locale}");
                    var validLocale = Enum.TryParse<Language>(request.Locale, out var locale);
                    if (!validLocale)
                    {
                        _logger.LogError($"Could not parse locale {request.Locale}");
                    }
                    else
                    {
                        _logger.LogInformation($"Successfully parsed locale: {locale}");
                        var translation = await client.TranslateAsync(
                            request.Text,
                            locale);
                        t = new Translation{Text = translation.Text};
                    }
                }
                catch (DeepLException ex)
                {
                    _logger.LogError("{@E}", ex);
                }
            }
            return TranslationMapper.ToResponse(t);
        }

        public Task<Translation[]> GetTranslations(Guid solutionId)
        {
            throw new NotImplementedException();
        }

        public Task<Translation> GetTranslation(Guid solutionId, string locale)
        {
            throw new NotImplementedException();
        }
    }
}