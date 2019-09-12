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
    }
}
