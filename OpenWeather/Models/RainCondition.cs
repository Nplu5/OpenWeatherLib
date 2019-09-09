using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather.Models
{

    public partial class RainCondition
    {
        [JsonProperty("3h")]
        public double ThreeHourProbability { get; set; }
    }
}
