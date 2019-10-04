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
            var openWeatherService = OpenWeatherService.CreateOpenWeatherService("ENTER API KEY HERE");
            var query = new OpenWeatherQuery(
                "Karlsruhe,de",
                new OpenWeatherDaySpecification(RelativeDay.NextDay,  DateTime.Today)                
            );
            var matches = await openWeatherService.GetForecasts(query).ConfigureAwait(false);

            foreach (var match in matches)
            {
                Console.WriteLine(match.ToString());
            }
        }
    }
}
