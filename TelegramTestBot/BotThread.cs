using System;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramTestBot
{
    static class BotThread
    {
        private static readonly string token = "385475873:AAHUeyt7Cy-K66ds5ICvvE-9geKZgY0bouY";

        private static readonly TelegramBotClient botClient;

        private static readonly WeatherService weatherProvider = new WeatherService();
        private static readonly LanguageParser languageParser = new LanguageParser();

        private static long lastChatId;

        public static event EventHandler<string> OnTextMessage;

        static BotThread()
        {
            botClient = new TelegramBotClient(token);

            botClient.OnMessage += BotClientOnMessage;
            botClient.StartReceiving();
        }

        public static Task SendMessageToLastSender(string message)
        {
            return SendMessage(lastChatId, message);
        }

        private static void BotClientOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.TextMessage)
            {
                HandleTextMessage(e.Message).ContinueWith(t =>
                {
                    Console.WriteLine(t.Exception);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private static async Task HandleTextMessage(Message message)
        {
            lastChatId = message.Chat.Id;
            OnOnTextMessage($"{message.From.FirstName} {message.From.LastName}: {message.Text}");

            string responseMessage = null;
            var city = languageParser.TryGetCity(message.Text);
            if (!string.IsNullOrEmpty(city))
            {
                var temperature = weatherProvider.GetTemperatureForecast(city);
                responseMessage = temperature != null ? 
                    $"Temperature in {city} is {temperature} now." : 
                    $"Got it, {city}. Hmm, could not fetch temperature for it. Please, try another city.";
            }
            else
            {
                responseMessage = "Sorry, I could not understand which city are you talking about. Please, rephrase your query for me :)";
            }
            
            await SendMessage(message.Chat.Id, responseMessage);
        }

        private static Task SendMessage(long chatId, string message)
        {
            return botClient.SendTextMessageAsync(chatId, message);
        }

        private static void OnOnTextMessage(string e)
        {
            OnTextMessage?.Invoke(null, e);
        }
    }
}