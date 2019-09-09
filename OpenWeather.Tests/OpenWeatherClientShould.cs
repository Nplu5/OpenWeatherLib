using System;
using System.Collections.Generic;
using System.Text;
using OpenWeather;
using Moq.Protected;
using Moq;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Linq;
using WeatherNotifier.Shared.Models;
using OpenWeather.Interfaces;

namespace OpenWeather.Tests
{
    public class OpenWeatherClientShould
    {
        private readonly string UrlToCall = "https://www.openweather.org/";

        [Fact]
        public async Task ForwardUrlToHttpClient()
        {
            const string expectedUrl = "http://www.Test.me/";
            bool IsUrlForwarded = false;
            var handlerMock = CreateMessageHandler(null, (HttpRequestMessage req, CancellationToken token) =>
                { IsUrlForwarded = expectedUrl.Contains(req.RequestUri.ToString(), StringComparison.OrdinalIgnoreCase); }
            );

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            await ClientUnderTest.GetForecastAsync(expectedUrl);

            Assert.True(IsUrlForwarded);
        }
        
        [Fact]
        public async Task ReturnForecastResponseObjectOnSuccessfullCall()
        {
            var handlerMock = CreateMessageHandler();

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            var forecast = (await ClientUnderTest.GetForecastAsync(UrlToCall))
                .DefaultIfEmpty(null)   // Use null as default as we want to have a successfull return
                .Single();

            Assert.IsType<ForecastResponse>(forecast);
        }

        [Fact]
        public async Task ReturnEmptyCollectionOnFalseFormattedUri()
        {
            var handlerMock = CreateMessageHandler();

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            var IsListEmpty = (await ClientUnderTest.GetForecastAsync("www.IllFormattedUrl"))
                .Count() == 0;

            Assert.True(IsListEmpty);
        }

        [Fact]
        public async Task ReturnEmptyCollectionWhenUrlProvidedIsNull()
        {
            // Is already covered by ReturnEmptyCollectionOnFalseFormattedUri Test but as the Exception differs from documentation it is in place till resolved
            var handlerMock = CreateMessageHandler();

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            var IsListEmpty = (await ClientUnderTest.GetForecastAsync(null))
                .Count() == 0;

            Assert.True(IsListEmpty);
        }

        [Fact]
        public async Task ReturnEmptyCollectionWhenUnexpectedJsonIsReceived()
        {
            var handlerMock = CreateMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{  \"json\": 1 }")
            });

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            var IsListEmpty = (await ClientUnderTest.GetForecastAsync(UrlToCall))
                .Count() == 0;

            Assert.True(IsListEmpty);
        }

        [Fact]
        public async Task ReturnEmptyCollectionOnHttpRequestException()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",  // Specify the name of the function to moq
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(() => { throw new HttpRequestException(); });

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            var IsListEmpty = (await ClientUnderTest.GetForecastAsync(UrlToCall))
                .Count() == 0;

            Assert.True(IsListEmpty);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.Gone)]
        [InlineData(HttpStatusCode.HttpVersionNotSupported)]
        public async Task ReturnEmptyCollectionIfStatusCodeIsNotSuccess(HttpStatusCode statusCode)
        {
            var handlerMock = CreateMessageHandler(new HttpResponseMessage() { StatusCode = statusCode });

            IClient ClientUnderTest = CreateTestClient(handlerMock.Object);
            var IsListEmpty = (await ClientUnderTest.GetForecastAsync(UrlToCall))
                .Count() == 0;

            Assert.True(IsListEmpty);
        }

        private IClient CreateTestClient(HttpMessageHandler handler)
        {
            return new OpenWeatherClient(handler);
        }

        private Mock<HttpMessageHandler> CreateMessageHandler(HttpResponseMessage desiredResponse = null, Action<HttpRequestMessage, CancellationToken> CallbackAction = null)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",  // Specify the name of the function to moq
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(
                desiredResponse is null
                ? new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(TestData.Forecast),
                }
                : desiredResponse
                )
               .Callback((HttpRequestMessage req, CancellationToken token) =>
               {
                   CallbackAction?.Invoke(req, token);
               });

            return handlerMock;
        }
    }
}
