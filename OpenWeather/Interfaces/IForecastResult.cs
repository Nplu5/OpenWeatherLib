using System.Collections.Generic;
using OpenWeather.Models;

namespace OpenWeather.Interfaces
{
    public interface IForecastResult
    {
        string Location { get; }
        IList<Forecast> ForecastData { get; }
        string ToString();
    }
}
