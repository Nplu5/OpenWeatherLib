using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenWeather;

namespace ConsoleOpenWeatherLib
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var openWeatherService = new OpenWeatherService("c9f817379c6fed669c718214bae83ab8");
            var query = new OpenWeatherQuery(
                new List<string>()
                {
                    "Karlsruhe,de",
                    "Stuttgart,de"
                },
                new List<OpenWeatherTimeSpecification>()
                {
                    new OpenWeatherTimeSpecification(RelativeDay.NextDay, new TimeSpan(7,30,0), DateTime.Today),
                    new OpenWeatherTimeSpecification(RelativeDay.NextDay, new TimeSpan(16,30,0), DateTime.Today)
                }
            );
            var matches = await openWeatherService.GetForecasts(query);

            foreach (var match in matches)
            {
                Console.WriteLine(match.ToString());
            }
        }
    }
}
