using System;
using System.Threading.Tasks;
using TranslationService.Dal.Models;

namespace TranslationService.Services.Services
{
    public interface ITranslationService
    {
        Task<TranslationResponse> Translate(TranslationRequest request);
        Task<Translation[]> GetTranslations(Guid solutionId);
        Task<Translation> GetTranslation(Guid solutionId, string locale);
    }
}