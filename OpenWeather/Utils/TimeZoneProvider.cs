using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather.Utils
{
    class TimeZoneProvider
    {
        internal static TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;
    }
}
