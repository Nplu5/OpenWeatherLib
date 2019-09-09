using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather.Models
{
    public partial class WeatherData
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        [JsonProperty("temp_min")]
        public double MinimumTemperature { get; set; }

        [JsonProperty("temp_max")]
        public double MaximumTemperature { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        [JsonProperty("sea_level")]
        public double SeaLevel { get; set; }

        [JsonProperty("grnd_level")]
        public double GroundLevel { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }
    }
}
