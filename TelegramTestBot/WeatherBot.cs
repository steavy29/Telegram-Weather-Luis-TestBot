using System;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramTestBot
{
    public class WeatherBot
    {
        private static readonly string token = "635867805:AAH3pblRSm_h3XFHRtVQBDTjbX0uwZS-Was";

        private readonly TelegramBotClient botClient;

        private readonly WeatherService weatherProvider = new WeatherService();
        private readonly LanguageParser languageParser = new LanguageParser();

        private long lastChatId;

        public event EventHandler<string> TextMessage;

        public WeatherBot()
        {
            botClient = new TelegramBotClient(token);

            botClient.OnMessage += BotClientOnMessage;
            botClient.StartReceiving();
        }

        public Task SendMessageToLastSender(string message)
        {
            return SendMessage(lastChatId, message);
        }

        private void BotClientOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {
                HandleTextMessage(e.Message).ContinueWith(t =>
                {
                    Console.WriteLine(t.Exception);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }

            if (e.Message.Type == MessageType.Location)
            {
                HandleLocationMessage(e.Message).ContinueWith(t =>
                {
                    Console.WriteLine(t.Exception);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private async Task HandleTextMessage(Message message)
        {
            lastChatId = message.Chat.Id;
            OnTextMessage($"{message.From.FirstName} {message.From.LastName}: {message.Text}");

            if (IsStartMessage(message))
            {
                await ReplyToGreeting(message);
                return;
            }

            var predicion = await languageParser.Predict(message.Text);
            if (!predicion.IntentMatched)
            {
                await ReplyToNotMatchedIntent(message);
                return;
            }

            if (predicion.Location == null)
            {
                await ReplyToNotDetectedLocation(message);
                return;
            }

            try
            {
                var weatherResponse = await weatherProvider.GetTemperatureForecast(predicion.Location);
                await ReplyWithWeather(message, predicion.Location, weatherResponse.Temperature);
            }
            catch (Exception ex)
            {
                await ReplyToFailedWeatherRequest(message, predicion.Location);
            }
        }

        private async Task HandleLocationMessage(Message message)
        {
            var locationString = $"(lon:{message.Location.Longitude},lat:{message.Location.Latitude})";

            try
            {
                var weatherResponse = await weatherProvider.GetTemperatureForecast(message.Location.Longitude, message.Location.Latitude);
                await ReplyWithWeather(message, weatherResponse.Location, weatherResponse.Temperature);
            }
            catch (Exception ex)
            {
                await ReplyToFailedWeatherRequest(message, locationString);
            }
        }

        private async Task ReplyWithWeather(Message message, string location, double temperature)
        {
            var responseMessage = $"Temperature in {location} is {temperature} now.";
            await SendMessage(message.Chat.Id, responseMessage);
        }

        private async Task ReplyToFailedWeatherRequest(Message message, string location)
        {
            var responseMessage = $"Hm, seems that out weather provider doesn't have records for {location}";
            await SendMessage(message.Chat.Id, responseMessage);
        }

        private async Task ReplyToNotDetectedLocation(Message message)
        {
            var responseMessage = "Sorry, I could not understand which location are you talking about.";
            await SendMessage(message.Chat.Id, responseMessage);
        }

        private async Task ReplyToNotMatchedIntent(Message message)
        {
            var responseMessage = "Were you asking about weather? Should have missed that, let's try again.";
            await SendMessage(message.Chat.Id, responseMessage);
        }

        private async Task ReplyToGreeting(Message message)
        {
            var responseMessage = $"Hi {message.From.FirstName}, you can ask me about weather in any city in the world(Provided, I know such one, khm).";

            await SendMessage(message.Chat.Id, responseMessage);
        }

        private bool IsStartMessage(Message message)
        {
            return string.Equals(message.Text, "/start");
        }

        private Task SendMessage(long chatId, string message)
        {
            return botClient.SendTextMessageAsync(chatId, message);
        }

        private void OnTextMessage(string e)
        {
            TextMessage?.Invoke(null, e);
        }
    }
}