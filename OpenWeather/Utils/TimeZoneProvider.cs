﻿using System;

namespace OpenWeather.Utils
{
    class TimeZoneProvider
    {
        internal static TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;
    }
}
