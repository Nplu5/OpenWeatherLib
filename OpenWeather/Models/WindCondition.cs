using Newtonsoft.Json;

namespace OpenWeather.Models
{
    public partial class WindCondition
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public double Deg { get; set; }
    }
}
