using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using OpenWeather.Interfaces;
using OpenWeather.Utils;

namespace OpenWeather.Models
{
    public class ForecastResult : IForecastResult
    {
        public string Location { get; }
        public IList<Forecast> ForecastData { get; }

        internal ForecastResult(string location)
        {
            Location = location;
            ForecastData = new List<Forecast>();
        }

        internal void AddRange(IEnumerable<Forecast> data)
        {
            foreach (var datum in data)
            {
                ForecastData.Add(datum);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Location);
            builder.AppendLine(":");

            foreach (var datum in ForecastData)
            {
                builder.Append(TimeZoneInfo.ConvertTimeFromUtc(datum.MeasureTime, TimeZoneProvider.TimeZone).ToString(CultureInfo.InvariantCulture));
                builder.Append(": ");
                builder.Append($"{datum.Data.Temperature}");
                builder.AppendLine("°C");
                builder.AppendLine("------------------------");
            }

            return builder.ToString();
        }
    }
}
