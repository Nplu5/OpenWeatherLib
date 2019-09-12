using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenWeather.Models;
using Xunit;

namespace OpenWeather.Tests
{
    public class OpenWeatherTimeSpecificationShould
    {
        [Fact]
        public void InitializeComparisonDateTimeWithNonDefaultValue()
        {
            RelativeDay relativeDay = RelativeDay.NextDay;
            TimeSpan timeSpan = new TimeSpan(8, 30, 0);
            DateTime referenceDateTime = new DateTime(2019, 08, 31, 18, 30, 34);

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.NotEqual(new DateTime(), SpecificationUnderTest.ComparisonDateTime);
        }

        [Fact]
        public void CreateComparisonDateTimeWithExpectedValues()
        {
            RelativeDay relativeDay = RelativeDay.NextDay;
            TimeSpan timeSpan = new TimeSpan(8, 30, 0);
            DateTime referenceDateTime = new DateTime(2019, 08, 31, 18, 30, 34);
            DateTime expectedComparisonDateTime = new DateTime(2019, 9, 1, 8, 30, 0);

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.Equal(expectedComparisonDateTime, SpecificationUnderTest.ComparisonDateTime);
        }

        [Theory]
        [MemberData(nameof(TestTimeSpans))]
        public void ThrowArgumentOutOfRangeExceptionWhenTimespanIsLongerThan24Hours(TimeSpan timeSpan)
        {
            RelativeDay relativeDay = RelativeDay.NextDay;
            DateTime referenceDateTime = new DateTime(2019, 08, 31, 18, 30, 34);

            Assert.Throws<ArgumentOutOfRangeException>(() => new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime));
        }

        public static IEnumerable<object[]> TestTimeSpans =>
            new List<object[]>
            {
                new object[] { new TimeSpan(25,0,0) },
                new object[] { new TimeSpan(24,0,0) },
                new object[] { new TimeSpan(23,70,0) },
                new object[] { new TimeSpan(23,59,61) }
            };

        private Forecast GetTestForecast()
        {
            // Available values:
            // 30.07.2019 21:00
            // 31.07.2019 00:00
            return ForecastResponse.FromJson(TestData.Forecast).Forecasts.Where(forecast =>
            {
                return forecast.MeasureTime.Equals(new DateTime(2019, 7, 30, 21, 0, 0));
            })
            .Single();
        }
        
        [Fact]
        public void ReturnTrueIfTimeDifferenceIsInRange()
        {
            var relativeDay = RelativeDay.NextDay;
            var timeSpan = new TimeSpan(22, 30, 0);
            var referenceDateTime = new DateTime(2019, 07, 29, 18, 30, 34);
            var testForecast = GetTestForecast();

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.True(SpecificationUnderTest.IsSatisfiedBy(testForecast));
        }

        [Fact]
        public void ReturnFalseIfTimeDifferenceIsOutOfRange()
        {
            var relativeDay = RelativeDay.NextDay;
            var timeSpan = new TimeSpan(19, 30, 0);
            var referenceDateTime = new DateTime(2019, 07, 28, 18, 30, 34);
            var testForecast = GetTestForecast();

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.False(SpecificationUnderTest.IsSatisfiedBy(testForecast));
        }

        [Fact]
        public void ReturnTrueIfTimeDifferenceMatchesLimitPositive()
        {
            var relativeDay = RelativeDay.NextDay;
            var timeSpan = new TimeSpan(22, 30, 0);
            var referenceDateTime = new DateTime(2019, 07, 29, 18, 30, 34);
            var testForecast = GetTestForecast();

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.True(SpecificationUnderTest.IsSatisfiedBy(testForecast));
        }

        [Fact]
        public void ReturnFalseIfTimeDifferenceMatchetsLimitNegatively()
        {
            var relativeDay = RelativeDay.NextDay;
            var timeSpan = new TimeSpan(19, 30, 0);
            var referenceDateTime = new DateTime(2019, 07, 29, 18, 30, 34);
            var testForecast = GetTestForecast();

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.False(SpecificationUnderTest.IsSatisfiedBy(testForecast));
        }

        [Fact]
        public void ReturnFalseIfPassedForecastIsNull()
        {
            var relativeDay = RelativeDay.NextDay;
            var timeSpan = new TimeSpan(21, 30, 0);
            var referenceDateTime = new DateTime(2019, 07, 29, 18, 30, 34);

            var SpecificationUnderTest = new OpenWeatherTimeSpecification(relativeDay, timeSpan, referenceDateTime);

            Assert.False(SpecificationUnderTest.IsSatisfiedBy(null));
        }
    }
}
