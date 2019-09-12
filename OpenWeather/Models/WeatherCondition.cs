using Newtonsoft.Json;

namespace OpenWeather.Models
{
    public partial class WeatherCondition
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("main")]
        public string Category { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }
}
