﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenWeather.Interfaces;
using OpenWeather.Models;
using OpenWeather.Utils;

namespace OpenWeather
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private IClient Client { get; set; }
        private IUrlProvider UrlProvider { get; set; }
        public static IOpenWeatherService CreateOpenWeatherService(string ApiKey)
        {
            return new OpenWeatherService(null,
                                          new OpenWeatherUrlProvider("http://api.openweathermap.org/data/2.5/forecast", ApiKey));
        }

        internal OpenWeatherService(IClient? client, IUrlProvider provider)
        {
            if (client == null)
                client = new OpenWeatherClient();

            Client = client;
            UrlProvider = provider;

            CreateDefaultUrlProvider();
        }
        
        public async Task<IEnumerable<IForecastResult>> GetForecasts(IQuery<string, Forecast> currentQuery)
        {
            if (currentQuery == null)
                throw new ArgumentNullException(ErrorMessages.ArgumentNullMessage(nameof(currentQuery)));

            var tasksToExecute = currentQuery.Queries.Select(queryLocation => UrlProvider.SetLocation(queryLocation).GetUriAsString())
                .Select(urlToCall => Client.GetForecastAsync(urlToCall));

            var results = await Task.WhenAll(tasksToExecute).ConfigureAwait(false);
            return results.Where(response => response.Any())
                .Select(response => response.Single())
                .Select(response =>
                {
                    var tempResult = new ForecastResult(response.City.DisplayName);
                    var Satisfied = response.Forecasts.Where(forecast => currentQuery.IsSatisfiedBy(forecast));
                    tempResult.AddRange(Satisfied);
                    return tempResult;
                }).
                Where(matchedForecast => matchedForecast.ForecastData.Any()) // Filter out, if no match was found
                .ToList();
        }

        private void CreateDefaultUrlProvider()
        {
            UrlProvider.SetLanguage(OpenWeatherUrlProvider.QueryLanguage.German)
                .SetMode(OpenWeatherUrlProvider.QueryMode.Json)
                .SetUnit(OpenWeatherUrlProvider.QueryUnit.Metric);
        }

        public void SetTimeZoneInfo(TimeZoneInfo timeZoneInfo)
        {
            TimeZoneProvider.TimeZone = timeZoneInfo;
        }
    }
}
