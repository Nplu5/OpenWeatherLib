using Newtonsoft.Json;

namespace OpenWeather.Models
{
    internal partial class City
    {
        [JsonProperty("id")]
        internal long Id { get; set; }

        [JsonProperty("name")]
        internal string DisplayName { get; set; }

        [JsonProperty("coord")]
        internal Coordinates Coordinates { get; set; }

        [JsonProperty("country")]
        internal string Country { get; set; }

        [JsonProperty("population")]
        internal long Population { get; set; }

        [JsonProperty("timezone")]
        internal long Timezone { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        internal City() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
    }
}
