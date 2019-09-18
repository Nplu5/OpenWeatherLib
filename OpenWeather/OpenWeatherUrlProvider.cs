using System;
using System.Runtime.CompilerServices;
using OpenWeather.Interfaces;

[assembly: InternalsVisibleTo("OpenWeather.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace OpenWeather
{
    internal class OpenWeatherUrlProvider : IUrlProvider
    {
        private const string LocationQueryParameterIdentifier = "q=";

        readonly UriBuilder _builder;

        internal OpenWeatherUrlProvider(string BaseUrl, string ApiKey)
        {
            _builder = new UriBuilder(BaseUrl);
            AppendQuery(string.Concat("APPID=", ApiKey));
        }

        string IUrlProvider.GetUriAsString()
        {
            return ((IUrlProvider)this).GetUri().AbsoluteUri;
        }

        Uri IUrlProvider.GetUri()
        {
            return _builder.Uri;
        }

        IUrlProvider IUrlProvider.SetLocation(string Location)
        {
            if (Location.Split(',').Length <= 1
                || Location.Split(',')[1].Length != 2)
                throw new ArgumentException("The location must be of the format of: {town},{countrycode}");

            if (_builder.Query.Contains(LocationQueryParameterIdentifier))
            {
                ReplaceLocationInQuery(Location);
                return this;
            }

            AppendQuery(string.Concat(LocationQueryParameterIdentifier, Location));
            return this;
        }


        IUrlProvider IUrlProvider.SetMode(QueryMode Mode)
        {
            const string queryIdentifier = "mode=";
            string queryParameter = string.Empty;
            switch (Mode)
            {
                case QueryMode.Json:
                    queryParameter = "json";
                    break;
                case QueryMode.Xml:
                    queryParameter = "xml";
                    break;
            }
            AppendQuery(string.Concat(queryIdentifier, queryParameter));
            return this;
        }

        IUrlProvider IUrlProvider.SetUnit(QueryUnit Unit)
        {
            const string queryIdentifier = "units=";
            string queryParameter = string.Empty;
            switch (Unit)
            {
                case QueryUnit.Imperial:
                    queryParameter = "imperial";
                    break;
                case QueryUnit.Metric:
                    queryParameter = "metric";
                    break;
                case QueryUnit.Kelvin:
                    return this;
            }
            AppendQuery(string.Concat(queryIdentifier, queryParameter));
            return this;
        }

        IUrlProvider IUrlProvider.SetLanguage(QueryLanguage Language)
        {
            const string queryIdentifier = "lang=";
            string queryParameter = string.Empty;
            switch (Language)
            {
                case QueryLanguage.German:
                    queryParameter = "de";
                    break;
                case QueryLanguage.English:
                    queryParameter = "en";
                    break;
                case QueryLanguage.French:
                    queryParameter = "fr";
                    break;
                case QueryLanguage.Turkish:
                    queryParameter = "tr";
                    break;
            }
            AppendQuery(string.Concat(queryIdentifier, queryParameter));
            return this;
        }

        internal enum QueryLanguage { German, English, French, Turkish }
        internal enum QueryMode { Json, Xml }
        internal enum QueryUnit { Metric, Imperial, Kelvin }

        private void AppendQuery(string QueryToAppend)
        {
            if (_builder.Query != null && _builder.Query.Length > 1)
                _builder.Query = _builder.Query.Substring(1) + "&" + QueryToAppend;
            else
                _builder.Query = QueryToAppend;
        }
        private void ReplaceLocationInQuery(string Location)
        {
            var startIndex = _builder.Query.IndexOf("q=");
            var endIndex = _builder.Query.IndexOf('&', startIndex);
            string subString = string.Empty;
            if (endIndex == -1)
            {
                subString = _builder.Query.Substring(startIndex + 2);
            }
            else
            {
                subString = _builder.Query.Substring(startIndex + 2, endIndex - startIndex - 2);
            }

            _builder.Query = _builder.Query.Replace(subString, Location);
        }
    }
}
