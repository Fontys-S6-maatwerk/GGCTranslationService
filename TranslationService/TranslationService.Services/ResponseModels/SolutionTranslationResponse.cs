namespace TranslationService.Services
{
    public class SolutionTranslationResponse
    {
        public Guid Id;
        public Guid SolutionId { get; set; }
        public string Name { get; set; }
        public string WeatherExtreme { get; set; }
        public string Description { get; set; }
        public string Locale { get; set; }
    }
}