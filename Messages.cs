using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bots.Extensions.Polling;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Newtonsoft.Json.Linq;

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

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            #region MainCode
            // Некоторые действия
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
                if (codeOfButton == "post")
                {
                    Console.WriteLine("You press button 1");
                    string telegramMessage = "You press button 1";
                    await botClient.SendMessage(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                }
                if (codeOfButton == "12")
                {
                    Console.WriteLine("You press button 2");
                    string telegramMessage = "Виберіть спеціальність:";
                    // await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                    InlineKeyboardMarkup inlineKeyBoard = new InlineKeyboardMarkup(
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

                    // await botClient.EditMessageCaptionAsync(chatId: update.CallbackQuery.Message.Chat.Id, caption: telegramMessage, messageId: update.CallbackQuery.Message.MessageId);
                    await _bot.EditMessageText(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: inlineKeyBoard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                }
            }

            #endregion
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
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
                        InlineKeyboardButton.WithCallbackData(text: "Події", callbackData: "post"),
                        // second button in row
                        InlineKeyboardButton.WithCallbackData(text: "Спеціальності", callbackData: "12"),
                    },
                    // second row
                    new[]
                    {
                        // first button in row
                        InlineKeyboardButton.WithUrl(text: "Сайт коледжу", url: "https://www.dktd.dp.ua/"),
                        InlineKeyboardButton.WithCallbackData("Залишити контактні дані")
                    },

                });

            Message sentMessage = await botClient.SendMessage(
                chatId: chatId,
                text: "Головне меню\nВиберіть пункт, який Вас зацікавив:\n ",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

    }
}
