using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenWeather.Models
{
    public partial class Forecast
    {
        [JsonProperty("dt")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime MeasureTime { get; internal set; }
        [JsonProperty("main")]
        public WeatherData Data { get; internal set; }

        [JsonProperty("weather")]
        public List<WeatherCondition> WeatherConditions { get; internal set; }

        [JsonProperty("wind")]
        public WindCondition Wind { get; internal set; }

        [JsonProperty("rain", NullValueHandling = NullValueHandling.Ignore)]
        public RainCondition Rain { get; internal set; }

        [JsonProperty("dt_txt")]
        public DateTimeOffset CalculationTime { get; internal set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        internal Forecast() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
    }

    internal class MicrosecondEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
                return;

            writer.WriteRawValue(((DateTime)value - _epoch).TotalSeconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
                return null;

            if (reader.Value == null)
                return null;

            return _epoch.AddSeconds((long)reader.Value);
        }
    }
}
