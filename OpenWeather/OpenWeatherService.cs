using OpenWeather.Interfaces;
using OpenWeather.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenWeather
{
    public class OpenWeatherService
    {
        internal IClient Client { get; set; } 
        internal IUrlProvider UrlProvider { get; set; } 
        public OpenWeatherService(string ApiKey) 
            : this(new OpenWeatherClient(),
                  new OpenWeatherUrlProvider("http://api.openweathermap.org/data/2.5/forecast", ApiKey)) {  }

        internal OpenWeatherService(IClient client, IUrlProvider provider)
        {
            Client = client;
            UrlProvider = provider;

            CreateDefaultUrlProvider();
        }

        public IEnumerable<Forecast> GetForecasts(IEnumerable<IQuery<string,Forecast>> queries)
        {
            var currentQuery = queries.First();
            UrlProvider.SetLocation(currentQuery.Queries.First());
            
            return new List<Forecast>();
        }

        private void CreateDefaultUrlProvider()
        {
            UrlProvider.SetLanguage(OpenWeatherUrlProvider.QueryLanguage.German)
                .SetMode(OpenWeatherUrlProvider.QueryMode.Json)
                .SetUnit(OpenWeatherUrlProvider.QueryUnit.Metric);
        }
    }
}
