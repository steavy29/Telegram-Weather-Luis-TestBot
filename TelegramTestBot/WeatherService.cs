using System;

namespace TelegramTestBot
{
    /// <summary>
    /// https://openweathermap.org/api
    /// </summary>
    class WeatherService
    {
        private static readonly string apiKey = "74d41e808242913d777779a206ec0c5f";

        public WeatherService()
        {
            WeatherNet.ClientSettings.SetApiKey(apiKey);
        }

        public double? GetTemperatureForecast(string city)
        {
            // TODO: fix this Api or find another that can be used only with city name
            // TODO: OpenWeather can handle requests without specifying country, but this Api package is dull
            var weatherClient = new WeatherNet.Clients.CurrentWeather();
            var currentWeather = weatherClient.GetByCityName(city, "Ukraine");
            if (!currentWeather.Success || !string.Equals(city, currentWeather.Item.City, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            
            return KelvinToCelcius(currentWeather.Item.Temp);
        }

        private static double KelvinToCelcius(double byKelvin)
        {
            return byKelvin - 273.15;
        }
    }
}