using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenWeather.Models;

namespace OpenWeather.Interfaces
{
    public interface IOpenWeatherService
    {
        Task<IEnumerable<ForecastResult>> GetForecasts(IQuery<string, Forecast> currentQuery);
    }
}
