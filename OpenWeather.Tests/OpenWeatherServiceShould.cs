using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using OpenWeather.Interfaces;
using WeatherNotifier.Shared.Models;

namespace OpenWeather.Tests
{
    public class OpenWeatherServiceShould
    {
        // Testdata time range: 30.07.2019 21:00 - 04.08.2019 18:00
        // Testdata for following locations: Stuttgart, Karlsruhe
        // TODO: Check if public interface can be extracted

        [Fact]
        public void BuildUrlDefaultUrlProviderDuringConstruction()
        {
            bool languageSet = false;
            bool modeSet = false;
            bool unitSet = false;
            var urlBuilder = new Mock<IUrlProvider>();
            urlBuilder.Setup(builder => builder.SetLanguage(It.IsAny<OpenWeatherUrlProvider.QueryLanguage>()))
                .Callback(() => languageSet = true)
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetMode(It.IsAny<OpenWeatherUrlProvider.QueryMode>()))
                .Callback(() => modeSet = true)
                .Returns(() => urlBuilder.Object ); 
            urlBuilder.Setup(builder => builder.SetUnit(It.IsAny<OpenWeatherUrlProvider.QueryUnit>()))
                .Callback(() => unitSet = true)
                .Returns(() =>  urlBuilder.Object );

            var serviceUnderTest = new OpenWeatherService(new Mock<IClient>().Object, urlBuilder.Object);

            Assert.True(languageSet && modeSet && unitSet);
        }

        [Fact]
        public void PassQueryLocationToUrlBuilderOnGetCall()
        {
            var Location = "Karlsruhe,de";
            string urlLocation = string.Empty;
            var urlBuilder = new Mock<IUrlProvider>();
            urlBuilder.Setup(builder => builder.SetLanguage(It.IsAny<OpenWeatherUrlProvider.QueryLanguage>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetMode(It.IsAny<OpenWeatherUrlProvider.QueryMode>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetUnit(It.IsAny<OpenWeatherUrlProvider.QueryUnit>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetLocation(It.IsAny<string>()))
                .Callback<string>(str =>
                {
                    urlLocation = str;
                })
                .Returns(() => urlBuilder.Object);
            var mockQuery = new Mock<IQuery<string, Forecast>>();
            mockQuery.Setup(query => query.Queries)
                .Returns(() => new List<string> { Location });

            var serviceUnderTest = new OpenWeatherService(new Mock<IClient>().Object, urlBuilder.Object);

            serviceUnderTest.GetForecasts(new List<IQuery<string,Forecast>>(){ mockQuery.Object });

            Assert.Contains(Location, urlLocation);
        }

        public void OneQueryWithOneLocationCallsCorrespondingRepositoryMethod()
        {

        }

        public void OneQueryWithOneLocationOneSpecReturnsMatchedForecast()
        {

        }

        public void OneQueryWithOneLocation2SpecsReturnsTwoForecasts()
        {

        }

        public void OneQueryWithOneLocation2SpecsReturnsEmptyForecast()
        {

        }

        public void OneQueryWith2LocationsAndoneSpecreturnsForecastForBothLocations()
        {

        }

        public void TwoQueryWithOneLocationAndTwoSpecsReturnsTwoForecasts()
        {

        }

        public void TwoQueryWithOneLocationAndTwoSpecsReturnsOnlyOneForecast()
        {

        }
    }
}
