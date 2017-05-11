using System.Linq;

using Microsoft.Cognitive.LUIS;

namespace TelegramTestBot
{
    class LuisConsts
    {
        public static readonly string CityPrebuiltEntityKey = "builtin.geography.city";
    }

    class LanguageParser
    {
        private static readonly string appId = "72bfc501-fd20-4e4d-9d95-f1c704a7d6f6";
        private static readonly string appKey = "7d2c4c4a4892497e84d4f5e143ca5d77";

        private static readonly double entityScoreThreshold = 0.5;

        private readonly LuisClient client;

        public LanguageParser()
        {
            client = new LuisClient(appId, appKey);
        }

        public string TryGetCity(string sentence)
        {
            var luisResult = client.Predict(sentence).Result;
            if(!luisResult.Entities.ContainsKey(LuisConsts.CityPrebuiltEntityKey))
                return null;

            var cityEntities = luisResult.Entities[LuisConsts.CityPrebuiltEntityKey];
            var highlyScoredCity = cityEntities.FirstOrDefault(e => e.Score > entityScoreThreshold);

            return highlyScoredCity?.Value;
        }
    }
}