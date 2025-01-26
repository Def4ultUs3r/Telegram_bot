using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Diagnostics.Metrics;
//using Telegram.Bots.Types;
//using Telegram.Bots.Types;
//using Telegram.Bots.Types;
//using Telegram.Bots.Types;

namespace Telegram_bot
{
    internal class Messages
    {
        private static ITelegramBotClient _bot;
        public Messages(ITelegramBotClient currentBot)
        {
            _bot = currentBot;

        }

        public void StartBot()
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {

            #region MainCode
            //  Всі дії
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));


            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    SendInline(botClient: botClient, chatId: message.Chat.Id, cancellationToken: cancellationToken);
                    return;
                }
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                string codeOfButton = update.CallbackQuery.Data;

                switch (codeOfButton)
                {
                    case "training":
                        {
                            Console.WriteLine("training");
                            string telegramMessage = "Підготовчі курси:\n";

                            var filePath = Path.Combine("Resources", "training.jpg");

                            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                await botClient.SendPhoto(
                                    chatId: update.CallbackQuery.Message.Chat.Id,
                                    photo: InputFile.FromStream(stream, "training.jpg"),
                                    caption: telegramMessage,
                                    cancellationToken: cancellationToken
                                );
                            }
                        }
                        break;

                    case "main_menu":
                        SendInline(botClient: botClient, chatId: update.CallbackQuery.Message.Chat.Id, cancellationToken: cancellationToken);
                        break;

                    case "map":
                        {
                            Console.WriteLine("show map");
                            string telegramMessage = "Ми знаходимось за адресою:\nпровулок Ушинського, 3, Дніпро, Дніпропетровська область, 49000";
                            await botClient.SendMessage(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                            double latitude = 48.461933773008624;  // Широта
                            double longitude = 35.03352385563664; // Довгота

                            await botClient.SendLocation(
                                chatId: update.CallbackQuery.Message.Chat.Id,
                                latitude: latitude,
                                longitude: longitude,
                                livePeriod: null, // Можна вказати livePeriod для відстеження
                                cancellationToken: cancellationToken
                            );

                            ShowMainMenuButton(botClient: botClient, chatId: update.CallbackQuery.Message.Chat.Id, cancellationToken);
                        }
                        break;

                    case "professions":
                        {
                            Console.WriteLine("You press button 2");
                            string telegramMessage = "Виберіть спеціальність:";
                            // await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                            Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup inlineKeyBoard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(
                                new[]
                                {
                            // first row
                            new[]
                            {
                                // first button in row
                                InlineKeyboardButton.WithCallbackData(text: "Button 3", callbackData: "post3"),
                                // second button in row
                                InlineKeyboardButton.WithCallbackData(text: "Button 4", callbackData: "post4"),
                            },

                                });

                            
                            await _bot.EditMessageText(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: inlineKeyBoard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                        }
                        break;

                    case "post3":
                        SendInline(botClient: botClient, chatId: update.CallbackQuery.Message.Chat.Id, cancellationToken: cancellationToken);
                        break;

                    default:
                        Console.WriteLine($"Default call");
                        break;
                }

                
                
                
                
            }

            #endregion
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async void ShowMainMenuButton(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            string telegramMessage = "Натисніть на кнопку👇, якщо бажаєте повернутись до головного меню\n";
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                // keyboard
                new[]
                {
                     new[]
                    {
                        // first button in row
                        InlineKeyboardButton.WithCallbackData(text: "Головне меню", callbackData: "main_menu")
                        
                    }
                });

            Message sentMessage = await botClient.SendMessage(
                chatId: chatId,
                text: telegramMessage,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        public static async void SendInline(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
                // keyboard
                new[]
                {
                    // first row
                    new[]
                    {
                        // first button in row
                        InlineKeyboardButton.WithCallbackData(text: "📚 Підготовчі курси", callbackData: "training"),
                        // second button in row
                        InlineKeyboardButton.WithCallbackData(text: "🤝 День відкритих дверей", callbackData: "door")                        
                    },
                    // second row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("📅 Графік навчання", callbackData: "plan"),
                        InlineKeyboardButton.WithCallbackData("🕰️ Розклад дзвінків", callbackData: "time")                        

                    },
                    //third row
                    new[]
                    {                        
                        InlineKeyboardButton.WithCallbackData("📖 Розклад занять(II семестр)", callbackData: "schedule")

                    },
                    
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "🧑‍🎓 Спеціальності", callbackData: "professions"),
                        InlineKeyboardButton.WithCallbackData("🪙 Платні послуги", callbackData: "money")

                    },
                    new[]
                    {
                        // first button in row
                        InlineKeyboardButton.WithUrl(text: "🌐 Сайт коледжу", url: "https://www.dktd.dp.ua/"),
                        InlineKeyboardButton.WithCallbackData("📇 Контакти", callbackData: "contacts")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "🧭 Як нас знайти", callbackData: "map")
                        
                    },

                });

            Message sentMessage = await botClient.SendMessage(
                chatId: chatId,
                text: "Головне меню\n\nВиберіть пункт, який Вас зацікавив:\n ",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

    }
}
