using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TranslationService.Dal.Models;

namespace TranslationService.Mappers
{
    public static class TranslationMapper
    {
        public static Translation TranslationDtoToTranslation(TranslationDto t)
        {
            return new()
            {
                Locale = t.Locale,
                SolutionId = t.SolutionId,
                Text = t.Text
            };
        }

        public static TranslationDto TranslationToTranslationDto(Translation t)
        {
            return new()
            {
                Locale = t.Locale,
                SolutionId = t.SolutionId,
                Text = t.Text
            };
        }

        public static TranslationDto[] TranslationToTranslationDto(IEnumerable<Translation> t)
        {
            return t.Select(TranslationToTranslationDto).ToArray();
        }
    }
}