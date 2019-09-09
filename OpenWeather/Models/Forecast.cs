using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather.Models
{
    public partial class Forecast
    {
        [JsonProperty("main")]
        public WeatherData Data { get; set; }

        [JsonProperty("weather")]
        public List<WeatherCondition> WeatherConditions { get; set; }

        [JsonProperty("wind")]
        public WindCondition Wind { get; set; }
        
        [JsonProperty("rain", NullValueHandling = NullValueHandling.Ignore)]
        public RainCondition Rain { get; set; }

        [JsonProperty("dt_txt")]
        public DateTimeOffset TimeOffset { get; set; }

    }
}
