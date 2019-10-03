using Newtonsoft.Json;

namespace OpenWeather.Models
{

    public partial class RainCondition
    {
        [JsonProperty("3h")]
        public double ThreeHourProbability { get; internal set; }

        internal RainCondition() { }
    }
}
