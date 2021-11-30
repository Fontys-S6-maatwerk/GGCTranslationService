using System;

namespace TranslationService.Dal.Models
{
    public class Translation
    {
        public Guid Id;
        public Guid SolutionId { get; set; }
        public string Text { get; set; }
        public string Locale { get; set; }
    }
}