using System;
using OpenWeather.Interfaces;
using Xunit;

namespace OpenWeather.Tests
{
    public class OpenWeatherUrlProviderShould
    {
        readonly string ApiKey = "91f9d294badfd96f86c06314a765b8cf_test";
        readonly string baseUrl = "http://api.openweathermap.org/data/2.5/forecast";

        [Fact]
        public void SetBaseUrlDuringConstruction()
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();

            Assert.Contains(baseUrl, ProviderUnderTest.GetUriAsString());
        }

        [Fact]
        public void SetApiKeyDuringConstruction()
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();

            Assert.Contains(string.Concat("APPID=", ApiKey), ProviderUnderTest.GetUriAsString());
        }

        [Fact]
        public void AppendApiKeyAsFirstQueryParamter()
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();

            Assert.Contains("?APPID=", ProviderUnderTest.GetUriAsString());
        }

        [Theory]
        [InlineData("Stuttgart,de")]
        [InlineData("Berlin,de")]
        [InlineData("Karlsruhe,de")]
        public void AddQueryParamterForTown(string LocationToSet)
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();
            ProviderUnderTest.SetLocation(LocationToSet);

            Assert.Contains($"q={LocationToSet}", ProviderUnderTest.GetUriAsString());
        }

        [Theory]
        [InlineData("Köln,de", "%C3%B6")]
        [InlineData("Osnabrück,de", "%C3%BC")]
        [InlineData("Münster,de", "%C3%BC")]
        public void EncodeUmlauteInQuery(string UmlautLocation, string ExpectedEncoding)
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();
            ProviderUnderTest.SetLocation(UmlautLocation);

            Assert.Contains(ExpectedEncoding, ProviderUnderTest.GetUriAsString());
        }

        [Theory]
        [InlineData("Köln,Deutschland")]
        [InlineData("Stuttgart:de")]
        [InlineData("de,Köln")]
        public void ThrowArgumentExceptionWhenLocationFormatIsWrong(string Location)
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();

            Assert.Throws<ArgumentException>(() => ProviderUnderTest.SetLocation(Location));
        }

        [Theory]
        [InlineData("mode=json", OpenWeatherUrlProvider.QueryMode.Json)]
        [InlineData("mode=xml", OpenWeatherUrlProvider.QueryMode.Xml)]
        internal void SetModeCorrespondingToQueryMode(string ExpectedMode, OpenWeatherUrlProvider.QueryMode ModeToSet)
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();
            ProviderUnderTest.SetMode(ModeToSet);

            Assert.Contains(ExpectedMode, ProviderUnderTest.GetUriAsString());
        }

        [Theory]
        [InlineData("units=metric", true, OpenWeatherUrlProvider.QueryUnit.Metric)]
        [InlineData("units=imperial", true, OpenWeatherUrlProvider.QueryUnit.Imperial)]
        [InlineData("units=", false, OpenWeatherUrlProvider.QueryUnit.Kelvin)]  // Does not contain units and thus uses kelvin -> how to cover in one test case
        internal void SetUnitCorrectly(string SearchString, bool ShouldContain, OpenWeatherUrlProvider.QueryUnit UnitToSet)
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();
            ProviderUnderTest.SetUnit(UnitToSet);

            if (ShouldContain)
            {
                Assert.Contains(SearchString, ProviderUnderTest.GetUriAsString());
            }
            else
            {
                Assert.DoesNotContain(SearchString, ProviderUnderTest.GetUriAsString());
            }
        }

        [Theory]
        [InlineData("lang=de", OpenWeatherUrlProvider.QueryLanguage.German)]
        [InlineData("lang=en", OpenWeatherUrlProvider.QueryLanguage.English)]
        [InlineData("lang=fr", OpenWeatherUrlProvider.QueryLanguage.French)]
        [InlineData("lang=tr", OpenWeatherUrlProvider.QueryLanguage.Turkish)]
        internal void SetLanguageCorrectly(string SearchString, OpenWeatherUrlProvider.QueryLanguage LanguageToSet)
        {
            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();
            ProviderUnderTest.SetLanguage(LanguageToSet);

            Assert.Contains(SearchString, ProviderUnderTest.GetUriAsString());
        }

        [Fact]
        internal void ProvideAFluidInterface()
        {
            const string Town = "Karlsruhe,de";
            const OpenWeatherUrlProvider.QueryMode Mode = OpenWeatherUrlProvider.QueryMode.Json;
            const OpenWeatherUrlProvider.QueryUnit Unit = OpenWeatherUrlProvider.QueryUnit.Metric;
            const OpenWeatherUrlProvider.QueryLanguage Language = OpenWeatherUrlProvider.QueryLanguage.German;

            IUrlProvider ProviderUnderTest = CreateDefaultUrlProvider();
            ProviderUnderTest.SetLocation(Town).SetMode(Mode).SetUnit(Unit).SetLanguage(Language);

            Assert.True(true);
        }

        [Fact]
        public void ReplaceOldLocationWithNewWhenCallingSetLocationTwiceWhenLastParameter()
        {
            const string firstTown = "Karlsruhe,de";
            const string secondTown = "Stuttgart,de";

            IUrlProvider providerUnderTest = CreateDefaultUrlProvider();
            providerUnderTest.SetLocation(firstTown);
            providerUnderTest.SetLocation(secondTown);
            var url = providerUnderTest.GetUriAsString();

            Assert.Contains(string.Concat("q=", secondTown), url);
            Assert.DoesNotContain(string.Concat("q=", firstTown), url);
        }

        [Fact]
        public void ReplaceOldLocationWithNewWhenCallingSetLocationTwiceWhenNotLastParameter()
        {
            const string firstTown = "Karlsruhe,de";
            const string secondTown = "Stuttgart,de";
            const OpenWeatherUrlProvider.QueryMode mode = OpenWeatherUrlProvider.QueryMode.Json;

            IUrlProvider providerUnderTest = CreateDefaultUrlProvider();
            providerUnderTest.SetLocation(firstTown)
                .SetMode(mode);
            providerUnderTest.SetLocation(secondTown);
            var url = providerUnderTest.GetUriAsString();

            Assert.Contains(string.Concat("q=", secondTown), url);
            Assert.DoesNotContain(string.Concat("q=", firstTown), url);
            Assert.Contains("mode=json", url);
        }

        private OpenWeatherUrlProvider CreateDefaultUrlProvider()
        {
            return new OpenWeatherUrlProvider(baseUrl, ApiKey);
        }
    }
}
