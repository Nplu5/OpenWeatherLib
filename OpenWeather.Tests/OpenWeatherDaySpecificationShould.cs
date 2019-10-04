using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenWeather.Models;
using Xunit;

namespace OpenWeather.Tests
{
    public class OpenWeatherDaySpecificationShould
    {
        [Fact]
        public void InitializeComparisonDateTimeWithNonDefaultValue()
        {
            var relativeDay = RelativeDay.NextDay;
            var reference = new DateTime(2018, 12, 1);

            var SpecificationUnderTest = new OpenWeatherDaySpecification(relativeDay, reference);

            Assert.NotEqual(new DateTime(), SpecificationUnderTest.ComparisonDateTime);
        }
        
        [Fact]
        public void AddRelativeDayToComparisonDateTime()
        {
            var relativeDay = RelativeDay.NextDay;
            var reference = new DateTime(2019, 10, 3);
            var expected = reference.AddDays(1.0);

            var SpecificationUnderTest = new OpenWeatherDaySpecification(relativeDay, reference);

            Assert.Equal(expected.Date, SpecificationUnderTest.ComparisonDateTime.Date);
        }

        private Forecast GetTestForecast(int trigger)
        {
            // Available values:
            // 30.07.2019 21:00
            // 31.07.2019 00:00
            return ForecastResponse.FromJson(TestData.Forecast).Forecasts.Where(forecast =>
            {
                if (trigger == 1)
                {
                    return forecast.MeasureTime.Equals(new DateTime(2019, 7, 30, 21, 0, 0));
                }
                else
                {
                    return forecast.MeasureTime.Equals(new DateTime(2019, 7, 31, 0, 0, 0));
                }
            })
            .Single();
        }

        [Fact]
        public void ReturnTrueForSameDay()
        {
            var relativeDay = RelativeDay.NextDay;
            var reference = new DateTime(2019, 07, 29);
            var testForecast = GetTestForecast(1);

            var specificationUnderTest = new OpenWeatherDaySpecification(relativeDay, reference);

            Assert.True(specificationUnderTest.IsSatisfiedBy(testForecast));
        }

        [Fact]
        public void ReturnFalseForDifferentDay()
        {
            var relativeDay = RelativeDay.NextDay;
            var reference = new DateTime(2019, 07, 29);
            var testForecast = GetTestForecast(2);

            var specificationUnderTest = new OpenWeatherDaySpecification(relativeDay, reference);

            Assert.False(specificationUnderTest.IsSatisfiedBy(testForecast));
        }

        [Fact]
        public void ReturnFalseWhenForecastIsNull()
        {
            var relativeDay = RelativeDay.NextDay;
            var reference = new DateTime(2019, 07, 29);

            var specificationUnderTest = new OpenWeatherDaySpecification(relativeDay, reference);

            Assert.False(specificationUnderTest.IsSatisfiedBy(null));
        }
    }
}
