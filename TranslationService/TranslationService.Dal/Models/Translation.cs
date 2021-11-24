using System;

namespace TranslationService.Dal.Models
{
    public class Translation
    {
        public Guid Id;
        public Guid SolutionId;
        public string Text;
        public string Locale { get; set; }
    }
}