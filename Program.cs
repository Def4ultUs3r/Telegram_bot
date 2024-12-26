using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bots.Extensions.Polling;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram_bot;

namespace TelegramTestProject
{

    class Program
    {
        
        

        static void Main(string[] args)
        {
            TelegramBotSettings stngs = new TelegramBotSettings();
            ITelegramBotClient _client = stngs.GetBotClient();
            
            Console.WriteLine("Bot is active " + _client.GetMe().Result.FirstName);
            
            Messages _messages = new Messages(_client);
            _messages.StartBot();
                        

            Console.ReadLine();
        }

        
    }
}