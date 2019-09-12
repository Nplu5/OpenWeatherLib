using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenWeather.Models
{
    public class ForecastResult
    {
        public string Location { get; }
        public IList<Forecast> ForecastData { get; }

        public ForecastResult(string location)
        {
            Location = location;
            ForecastData = new List<Forecast>();
        }

        public void AddRange(IEnumerable<Forecast> data)
        {
            foreach(var datum in data)
            {
                ForecastData.Add(datum);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Location);
            builder.AppendLine(":");

            foreach(var datum in ForecastData)
            {
                builder.Append(datum.MeasureTime.ToLocalTime().ToString());
                builder.Append(": ");
                builder.Append($"{datum.Data.Temperature}");
                builder.AppendLine("°C");
                builder.AppendLine("------------------------");
            }


            return builder.ToString();
        }
    }
}
