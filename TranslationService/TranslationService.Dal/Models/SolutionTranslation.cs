using System;

namespace TranslationService.Dal.Models
{
    public class SolutionTranslation
    {
        
        public Guid Id;
        public Guid SolutionId { get; set; }
        public string Name { get; set; }
        public string WeatherExtreme { get; set; }
        public string Description { get; set; }
        public string Locale { get; set; }
    }
}