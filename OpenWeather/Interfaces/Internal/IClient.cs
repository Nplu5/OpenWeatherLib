using System.Threading.Tasks;
using OpenWeather.Helper;
using OpenWeather.Models;

namespace OpenWeather.Interfaces
{
    internal interface IClient
    {
        Task<Maybe<ForecastResponse>> GetForecastAsync(string url);
    }
}
