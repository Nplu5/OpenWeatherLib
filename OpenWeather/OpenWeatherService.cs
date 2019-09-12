using OpenWeather.Interfaces;
using OpenWeather.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWeather
{
    public class OpenWeatherService
    {
        internal IClient Client { get; set; }
        internal IUrlProvider UrlProvider { get; set; }
        public OpenWeatherService(string ApiKey)
            : this(new OpenWeatherClient(),
                  new OpenWeatherUrlProvider("http://api.openweathermap.org/data/2.5/forecast", ApiKey))
        { }

        internal OpenWeatherService(IClient client, IUrlProvider provider)
        {
            Client = client;
            UrlProvider = provider;

            CreateDefaultUrlProvider();
        }

        public async Task<IEnumerable<ForecastResult>> GetForecasts(IQuery<string, Forecast> currentQuery)
        {
            var tasksToExecute = currentQuery.Queries.Select(queryLocation => UrlProvider.SetLocation(queryLocation).GetUriAsString())
                .Select(urlToCall => Client.GetForecastAsync(urlToCall));

            var results = await Task.WhenAll(tasksToExecute);
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
    }
}
