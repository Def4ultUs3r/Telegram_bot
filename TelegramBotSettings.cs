using Telegram.Bot;

namespace TelegramTestProject
{
    internal class TelegramBotSettings
    {
        private static string _token = "7614779821:AAGfN2kTjZrEE1f8FHTw6u0dKU_ge3DKTZc";
        private static ITelegramBotClient _bot;
        public  TelegramBotSettings()
        {
            _bot = new TelegramBotClient(_token);
            
        }

        public ITelegramBotClient GetBotClient()
        {
            if (_bot != null)
                return _bot;
            else
            {
                _bot = new TelegramBotClient(_token);
                return _bot;
            }
        }
    }
}