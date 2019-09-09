using OpenWeather.Helper;
using OpenWeather.Models;
using System.Threading.Tasks;

namespace OpenWeather.Interfaces
{
    internal interface IClient
    {
        Task<Maybe<ForecastResponse>> GetForecastAsync(string url);
    }
}
