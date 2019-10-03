using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenWeather.Models
{
    internal partial class ForecastResponse
    {
        [JsonProperty("cnt", Required = Required.Always)]
        internal long NumberofForecasts { get; set; }

        [JsonProperty("list", Required = Required.Always)]
        internal List<Forecast> Forecasts { get; set; }

        [JsonProperty("city", Required = Required.Always)]
        internal City City { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        internal ForecastResponse() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
    }

    internal partial class ForecastResponse
    {
        public static ForecastResponse FromJson(string json) => JsonConvert.DeserializeObject<ForecastResponse>(json, Converter.Settings);
    }

    internal static class Serialize
    {
        public static string ToJson(this ForecastResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        internal static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
