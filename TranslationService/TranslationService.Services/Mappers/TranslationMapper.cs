using TranslationService.Dal.Models;

namespace TranslationService.Services.Mappers
{
    public class TranslationMapper
    {
        public static Translation FromRequest(TranslationRequest t)
        {
            return new()
            {
                Text = t.Text
            };
        }

        public static TranslationResponse ToResponse(Translation t)
        {
            return new()
            {
                Text = t.Text
            };
        }
    }
}