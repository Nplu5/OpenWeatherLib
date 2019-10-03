using Newtonsoft.Json;

namespace OpenWeather.Models
{
    internal partial class Coordinates
    {
        [JsonProperty("lat")]
        internal double Latitude { get; set; }

        [JsonProperty("lon")]
        internal double Longitude { get; set; }
    }
}
