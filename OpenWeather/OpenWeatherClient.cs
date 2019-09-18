using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenWeather.Helper;
using OpenWeather.Interfaces;
using OpenWeather.Models;

namespace OpenWeather
{
    internal class OpenWeatherClient : HttpClient, IClient
    {
        internal OpenWeatherClient(HttpMessageHandler handler) : base(handler) { }

        internal OpenWeatherClient() : base() { }

        async Task<Maybe<ForecastResponse>> IClient.GetForecastAsync(string url)
        {
            try
            {
                var response = await base.GetAsync(url);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return new Maybe<ForecastResponse>();

                var deserializedResponse = ForecastResponse.FromJson(await response.Content.ReadAsStringAsync());
                return new Maybe<ForecastResponse>(deserializedResponse);
            }
            catch (HttpRequestException ex)
            {
                return new Maybe<ForecastResponse>()
                {
                    ErrorMessage = $"{ex.GetType().Name}: {ex.Message}"
                };
            }
            catch (InvalidOperationException ex)
            {
                return new Maybe<ForecastResponse>()
                {
                    ErrorMessage = $"{ex.GetType().Name}: {ex.Message}"
                };
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                return new Maybe<ForecastResponse>()
                {
                    ErrorMessage = $"{ex.GetType().Name}: {ex.Message}"
                };
            }
        }
    }
}
