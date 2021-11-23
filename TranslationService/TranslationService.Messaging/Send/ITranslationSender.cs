using TranslationService.Services;

namespace TranslationService.MQ.Send
{
    public interface ITranslationSender
    {
        void Send(TranslationResponse obj);
    }
}