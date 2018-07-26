using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TelegramTestBot
{
    class WeatherService
    {
        private static readonly string appId = "74d41e808242913d777779a206ec0c5f";

        private readonly OpenWeatherClient openWeatherClient;

        public WeatherService()
        {
            openWeatherClient = new OpenWeatherClient(appId);
        }

        public async Task<WeatherResponse> GetTemperatureForecast(string location)
        {
            var weatherResponse = await openWeatherClient.GetTemperature(location);

            weatherResponse.Temperature = KelvinToCelcius(weatherResponse.Temperature);

            return weatherResponse;
        }

        public async Task<WeatherResponse> GetTemperatureForecast(float lon, float lat)
        {
            var weatherResponse = await openWeatherClient.GetTemperature(lon, lat);

            weatherResponse.Temperature = KelvinToCelcius(weatherResponse.Temperature);

            return weatherResponse;
        }

        private static double KelvinToCelcius(double byKelvin)
        {
            return byKelvin - 273.15;
        }
    }

    public class WeatherResponse
    {
        public double Temperature { get; set; }

        public string Location { get; set; }
    }

    public class OpenWeatherClient
    {
        private static readonly string apiUrlFormat = "http://api.openweathermap.org/data/2.5/weather?appid={0}&";

        private readonly HttpClient httpClient = new HttpClient();
        private readonly string appId;
        
        public OpenWeatherClient(string appId)
        {
            this.appId = appId;
        }

        public Task<WeatherResponse> GetTemperature(float lon, float lat)
        {
            var url = string.Format(apiUrlFormat + "lon={1}&lat={2}", appId, lon, lat);
            return GetTemperatureByFullUrl(url);
        }

        public Task<WeatherResponse> GetTemperature(string query)
        {
            var url = string.Format(apiUrlFormat + "q={1}", appId, query);
            return GetTemperatureByFullUrl(url);
        }

        public async Task<WeatherResponse> GetTemperatureByFullUrl(string url)
        {
            var response = await httpClient.GetAsync(url);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(responseBody);

            var temperature = responseObject["main"]["temp"].Value<double>();
            var name = responseObject["name"].Value<string>();

            return new WeatherResponse
            {
                Temperature = temperature,
                Location = name
            };
        }
    }
}

