using Newtonsoft.Json;

namespace OpenWeather.Models
{
    public partial class WeatherCondition
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("main")]
        public string Category { get; internal set; }

        [JsonProperty("description")]
        public string Description { get; internal set; }

        internal WeatherCondition() { }
    }
}
