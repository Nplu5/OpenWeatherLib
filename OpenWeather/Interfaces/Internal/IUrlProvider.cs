using System;
using static OpenWeather.OpenWeatherUrlProvider;

namespace OpenWeather.Interfaces
{
    internal interface IUrlProvider
    {
        string GetUriAsString();
        Uri GetUri();
        IUrlProvider SetLocation(string Location);
        IUrlProvider SetMode(QueryMode Mode);
        IUrlProvider SetUnit(QueryUnit Unit);
        IUrlProvider SetLanguage(QueryLanguage Language);
    }
}
