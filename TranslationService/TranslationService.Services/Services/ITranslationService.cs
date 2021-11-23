using System.Threading.Tasks;

namespace TranslationService.Services.Services
{
    public interface ITranslationService
    {
        Task<TranslationResponse> Translate(TranslationRequest request);
    }
}