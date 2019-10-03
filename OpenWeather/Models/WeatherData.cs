using Newtonsoft.Json;

namespace OpenWeather.Models
{
    public partial class WeatherData
    {
        [JsonProperty("temp")]
        public double Temperature { get; internal set; }

        [JsonProperty("temp_min")]
        public double MinimumTemperature { get; internal set; }

        [JsonProperty("temp_max")]
        public double MaximumTemperature { get; internal set; }

        [JsonProperty("pressure")]
        public double Pressure { get; internal set; }

        [JsonProperty("sea_level")]
        public double SeaLevel { get; internal set; }

        [JsonProperty("grnd_level")]
        public double GroundLevel { get; internal set; }

        [JsonProperty("humidity")]
        public long Humidity { get; internal set; }

        internal WeatherData() { }
    }
}
