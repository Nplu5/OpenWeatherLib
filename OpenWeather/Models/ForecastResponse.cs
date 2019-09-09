using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenWeather.Models
{
    partial class ForecastResponse
    {
        [JsonProperty("cnt", Required = Required.Always)]
        public long NumberofForecasts { get; set; }

        [JsonProperty("list", Required = Required.Always)]
        public List<Forecast> Forecasts { get; set; }

        [JsonProperty("city", Required = Required.Always)]
        public City City { get; set; }
    }

    public partial class ForecastResponse
    {
        public static ForecastResponse FromJson(string json) => JsonConvert.DeserializeObject<ForecastResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ForecastResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
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
