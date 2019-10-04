using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Language;
using OpenWeather.Interfaces;
using OpenWeather.Models;
using OpenWeather.Utils;
using Xunit;
using static OpenWeather.OpenWeatherUrlProvider;

namespace OpenWeather.Tests
{
    public class OpenWeatherServiceShould
    {
        // Testdata time range: 30.07.2019 21:00 - 04.08.2019 18:00
        // Testdata for following locations: Stuttgart, Karlsruhe

        private Mock<IUrlProvider> CreateFluentUrlProvider(string ReturnUrl = null)
        {
            var urlBuilder = new Mock<IUrlProvider>();
            urlBuilder.Setup(builder => builder.SetLanguage(It.IsAny<QueryLanguage>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetMode(It.IsAny<QueryMode>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetUnit(It.IsAny<QueryUnit>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.SetLocation(It.IsAny<string>()))
                .Returns(() => urlBuilder.Object);
            urlBuilder.Setup(builder => builder.GetUriAsString())
                .Returns(() =>
                {
                    return string.IsNullOrEmpty(ReturnUrl)
                        ? string.Empty
                        : ReturnUrl;
                });

            return urlBuilder;
        }

        private Mock<IUrlProvider> CreateFluentUrlProviderWithReturnUrl()
        {
            return CreateFluentUrlProvider("http://api.openweathermap.org/data/2.5/forecast");
        }

        private Mock<IClient> CreateClient(List<Helper.Maybe<ForecastResponse>> responses = null)
        {
            var client = new Mock<IClient>();
            if (responses is null)
            {
                client.Setup(cli => cli.GetForecastAsync(It.IsAny<string>()))
                    .Returns(() => Task.FromResult(new Helper.Maybe<ForecastResponse>()));
                return client;
            }

            var setup = client.SetupSequence(cli => cli.GetForecastAsync(It.IsAny<string>()));
            ISetupSequentialResult<Task<Helper.Maybe<ForecastResponse>>> flow = null;
            foreach (var response in responses)
            {
                if (flow is null)
                {
                    flow = setup.Returns(() => Task.FromResult(response));
                }
                else
                {
                    flow = flow.Returns(() => Task.FromResult(response));
                }
            }

            return client;
        }

        [Fact]
        public void BuildUrlDefaultUrlProviderDuringConstruction()
        {
            var urlBuilder = CreateFluentUrlProvider();

            var serviceUnderTest = new OpenWeatherService(new Mock<IClient>().Object, urlBuilder.Object);

            urlBuilder.Verify(builder => builder.SetLanguage(It.IsAny<QueryLanguage>()), Times.Once());
            urlBuilder.Verify(builder => builder.SetMode(It.IsAny<QueryMode>()), Times.Once());
            urlBuilder.Verify(builder => builder.SetUnit(It.IsAny<QueryUnit>()), Times.Once());
        }

        [Fact]
        public async Task CallSetLocationOnceWithOneQueryLocationOnGetForecast()
        {
            var location = "Karlsruhe,de";
            string urlLocation = string.Empty;
            var urlBuilder = CreateFluentUrlProvider();
            var mockQuery = CreateQuery(location);
            var client = CreateClient();

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);
            await serviceUnderTest.GetForecasts(mockQuery.Object);

            urlBuilder.Verify(builder => builder.SetLocation(It.IsAny<string>()), Times.Once);
        }

        private Mock<IQuery<string, Forecast>> CreateQuery(string location)
        {
            return CreateQuery(new List<string>() { location });
        }

        private Mock<IQuery<string, Forecast>> CreateQuery(IEnumerable<string> locations)
        {
            var mockQuery = new Mock<IQuery<string, Forecast>>();
            mockQuery.Setup(query => query.Queries)
                .Returns(() => locations);
            return mockQuery;
        }

        [Fact]
        public async Task BuildUrlFromEveryLocationInQueryOnGetForecast()
        {
            string firstLocation = "Karlsruhe,de";
            string secondLocation = "Stuttgart,de";
            var urlBuilder = CreateFluentUrlProvider();
            var client = CreateClient();

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);
            await serviceUnderTest.GetForecasts(CreateQuery(new List<string>() { firstLocation, secondLocation }).Object);

            urlBuilder.Verify(builder => builder.SetLocation(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CallClientWithUrlFromUrlBuilderWhenQueriedWithOneQueryLocation()
        {
            var Location = "Karlsruhe,de";
            var urlBuilder = CreateFluentUrlProvider("http://api.openweathermap.org/data/2.5/forecast");
            var client = CreateClient();
            var mockQuery = CreateQuery(Location);

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            await serviceUnderTest.GetForecasts(mockQuery.Object);

            client.Verify(cli => cli.GetForecastAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CallClientWithEachUrlFromurlBuilderForEachQueryLocation()
        {
            string firstLocation = "Karlsruhe,de";
            string secondLocation = "Stuttgart,de";
            var urlBuilder = CreateFluentUrlProviderWithReturnUrl();
            var client = CreateClient();
            var mockQuery = CreateQuery(new List<string>() { firstLocation, secondLocation });

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            await serviceUnderTest.GetForecasts(mockQuery.Object);

            client.Verify(cli => cli.GetForecastAsync(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public async Task ReturnMatchingForecastWhenQueryWasSatisfiedByForecast()
        {
            string location = "Karlsruhe,de";
            var urlBuilder = CreateFluentUrlProviderWithReturnUrl();
            var client = CreateClient(new List<Helper.Maybe<ForecastResponse>>() { new Helper.Maybe<ForecastResponse>(ForecastResponse.FromJson(TestData.ForecastResponseKarlsruhe)) });
            var mockQuery = CreateQuery(location);
            mockQuery.Setup(query => query.IsSatisfiedBy(It.IsAny<Forecast>()))
                .Returns((Forecast forecast) => forecast.MeasureTime.Equals(new DateTime(2019, 7, 30, 21, 0, 0)));

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            var result = await serviceUnderTest.GetForecasts(mockQuery.Object);

            Assert.True(result.Any());
        }

        [Fact]
        public async Task ReturnsIEnumerableForGetForecast()
        {
            string location = "Karlsruhe,de";
            var urlBuilder = CreateFluentUrlProviderWithReturnUrl();
            var client = CreateClient(new List<Helper.Maybe<ForecastResponse>>() { new Helper.Maybe<ForecastResponse>(ForecastResponse.FromJson(TestData.ForecastResponseKarlsruhe)) });
            var mockQuery = CreateQuery(location);
            mockQuery.Setup(query => query.IsSatisfiedBy(It.IsAny<Forecast>()))
                .Returns((Forecast forecast) => forecast.MeasureTime.Equals(new DateTimeOffset(2019, 7, 30, 21, 0, 0, new TimeSpan())));

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            var result = await serviceUnderTest.GetForecasts(mockQuery.Object);

            Assert.IsAssignableFrom<IEnumerable<ForecastResult>>(result);
        }

        [Fact]
        public async Task ReturnEmptyIEnumerableWhenNoMatchFound()
        {
            string location = "Karlsruhe,de";
            var urlBuilder = CreateFluentUrlProviderWithReturnUrl();
            var client = CreateClient(new List<Helper.Maybe<ForecastResponse>>() { new Helper.Maybe<ForecastResponse>(ForecastResponse.FromJson(TestData.ForecastResponseKarlsruhe)) });
            var mockQuery = CreateQuery(location);
            mockQuery.Setup(query => query.IsSatisfiedBy(It.IsAny<Forecast>()))
                .Returns((Forecast forecast) => false);

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            var result = await serviceUnderTest.GetForecasts(mockQuery.Object);

            Assert.False(result.Any());
        }

        [Fact]
        public async Task OneQueryWithOneLocationAndTwoHitsReturnsOneForecastResultWithTwoElements()
        {
            string location = "Karlsruhe,de";
            var urlBuilder = CreateFluentUrlProviderWithReturnUrl();
            var client = CreateClient(new List<Helper.Maybe<ForecastResponse>>() { new Helper.Maybe<ForecastResponse>(ForecastResponse.FromJson(TestData.ForecastResponseKarlsruhe)) });
            var mockQuery = CreateQuery(location);
            mockQuery.Setup(query => query.IsSatisfiedBy(It.IsAny<Forecast>()))
                .Returns((Forecast forecast) =>
                {
                    return forecast.MeasureTime.Equals(new DateTime(2019, 7, 30, 21, 0, 0))
                        || forecast.MeasureTime.Equals(new DateTime(2019, 7, 31, 9, 0, 0));
                });

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            var result = await serviceUnderTest.GetForecasts(mockQuery.Object);

            Assert.True(result.First().ForecastData.Count == 2);
        }

        [Fact]
        public async Task ReturnTwoForecastResultsWhenGivenTwoLocationsInQuery()
        {
            string firstLocation = "Karlsruhe,de";
            string secondLocation = "Stuttgart,de";
            var urlBuilder = CreateFluentUrlProviderWithReturnUrl();
            var client = CreateClient(new List<Helper.Maybe<ForecastResponse>>()
            {
                new Helper.Maybe<ForecastResponse>(ForecastResponse.FromJson(TestData.ForecastResponseKarlsruhe)),
                new Helper.Maybe<ForecastResponse>(ForecastResponse.FromJson(TestData.ForecastResponseStuttgart))
            });
            var mockQuery = CreateQuery(new List<string>() { firstLocation, secondLocation });
            mockQuery.Setup(query => query.IsSatisfiedBy(It.IsAny<Forecast>()))
                .Returns((Forecast forecast) => forecast.MeasureTime.Equals(new DateTime(2019, 7, 30, 21, 0, 0)));

            var serviceUnderTest = new OpenWeatherService(client.Object, urlBuilder.Object);

            var result = await serviceUnderTest.GetForecasts(mockQuery.Object);

            Assert.True(result.Count() == 2);
        }

        [Fact]
        public void SetTimeZoneInfoForUseInLibrary()
        {
            var expectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azores Standard Time");
            var urlBuilder = CreateFluentUrlProvider();
            var client = CreateClient();
            var serviceUnderTest = (IOpenWeatherService)new OpenWeatherService(client.Object, urlBuilder.Object);
            serviceUnderTest.SetTimeZoneInfo(expectedTimeZone);

            Assert.Equal(expectedTimeZone, TimeZoneProvider.TimeZone);
            serviceUnderTest.SetTimeZoneInfo(TimeZoneInfo.Utc);
        }
    }
}
