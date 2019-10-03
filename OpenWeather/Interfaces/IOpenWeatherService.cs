using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenWeather.Models;

namespace OpenWeather.Interfaces
{
    public interface IOpenWeatherService
    {
        Task<IEnumerable<IForecastResult>> GetForecasts(IQuery<string, Forecast> currentQuery);

        void SetTimeZoneInfo(TimeZoneInfo timeZoneInfo);
    }
}
