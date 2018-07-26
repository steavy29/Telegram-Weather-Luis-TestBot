using System.Linq;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;

namespace TelegramTestBot
{
    class LanguageParser
    {
        private static readonly string WeatherIntent = "GetWeather";
        private static readonly string LocationEntity = "location";

        private static readonly string appId = "72bfc501-fd20-4e4d-9d95-f1c704a7d6f6";
        private static readonly string appKey = "7d2c4c4a4892497e84d4f5e143ca5d77";

        private static readonly double scoreThreshold = 0.8;

        private readonly LuisClient client;

        public LanguageParser()
        {
            client = new LuisClient(appId, appKey);
        }

        public async Task<PredictionResponse> Predict(string sentence)
        {
            var luisResult = await client.Predict(sentence);
            var weatherIntent = luisResult.Intents.FirstOrDefault(i => i.Name == WeatherIntent);
            if (weatherIntent == null || weatherIntent.Score < scoreThreshold)
            {
                return new PredictionResponse(false);
            }

            if (!luisResult.Entities.ContainsKey(LocationEntity))
            {
                return new PredictionResponse(true, null);
            }

            var cityEntities = luisResult.Entities[LocationEntity];
            var highlyScoredCity = cityEntities.FirstOrDefault(e => e.Score > scoreThreshold);
            if (highlyScoredCity == null)
            {
                return new PredictionResponse(true, null);
            }

            return new PredictionResponse(true, highlyScoredCity.Value);
        }

        public class PredictionResponse
        {
            public bool IntentMatched { get; set; }

            public string Location { get; set; }

            public PredictionResponse(bool intentMatched, string location = null)
            {
                IntentMatched = intentMatched;
                Location = location;
            }
        }
    }
}