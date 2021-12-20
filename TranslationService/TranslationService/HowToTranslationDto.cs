using System;

namespace TranslationService
{
    public class HowToTranslationDto : SolutionTranslationDto
    {
        public string Introduction { get; set; }
        public string Difficulty { get; set; }
        public List<string> Materials { get; set; }
        public List<string> Tools { get; set; }
        public List<string> Steps { get; set; }    
    }
}