using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace OpenWeather.Models
{
    public partial class Forecast
    {
        [JsonProperty("dt")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime MeasureTime { get; set; }
        [JsonProperty("main")]
        public WeatherData Data { get; set; }

        [JsonProperty("weather")]
        public List<WeatherCondition> WeatherConditions { get; set; }

        [JsonProperty("wind")]
        public WindCondition Wind { get; set; }
        
        [JsonProperty("rain", NullValueHandling = NullValueHandling.Ignore)]
        public RainCondition Rain { get; set; }

        [JsonProperty("dt_txt")]
        public DateTimeOffset CalculationTime { get; set; }

    }

    public class MicrosecondEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalSeconds.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return _epoch.AddSeconds((long)reader.Value);
        }
    }
}
