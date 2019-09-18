using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using OpenWeather.Interfaces;
using OpenWeather.Models;
using Xunit;

namespace OpenWeather.Tests
{
    public class OpenWeatherQueryShould
    {
        [Fact]
        public void TakeOneLocationAndOneSpecInCtorAndCheckForNullOfSpec()
        {
            string location = "Karlsruhe";
            ISpecification<Forecast> spec = null;

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherQuery(location, spec));
        }

        [Fact]
        public void TakeOneLocationAndOneSpecInCtorAndCheckForNullOfLocation()
        {
            string location = null;
            ISpecification<Forecast> spec = new Mock<ISpecification<Forecast>>().Object;

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherQuery(location, spec));
        }

        [Fact]
        public void TakeOneLocationAndEnumerableOfSpecAndCheckForEmptyString()
        {
            string location = string.Empty;
            ISpecification<Forecast> spec = new Mock<ISpecification<Forecast>>().Object;

            Assert.Throws<ArgumentException>(() => new OpenWeatherQuery(location, spec));
        }

        [Fact]
        public void TakeOneLocationAndOneSpecInCtroAndAddToProperties()
        {
            string location = "Karlsruhe";
            ISpecification<Forecast> spec = new Mock<ISpecification<Forecast>>().Object;

            var QueryUnderTest = new OpenWeatherQuery(location, spec);

            var IsLocationAdded = QueryUnderTest.Queries.Count() > 0;
            var IsSpecAdded = QueryUnderTest.Queries.Count() > 0;

            Assert.True(IsSpecAdded);
            Assert.True(IsLocationAdded);
        }

        [Fact]
        public void TakeOneLocationAndEnumerableAndCheckForNull()
        {
            string location = "Karlsrueh";
            List<ISpecification<Forecast>> specs = null;

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherQuery(location, specs));
        }

        [Fact]
        public void TakeOneLocationAndEnumerableAndCheckForItemsNull()
        {
            string location = "Karlsrueh";
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            specs.Add(null);

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherQuery(location, specs));
        }

        [Fact]
        public void TakeOneLocationAndEnumerableAndCheckForEmptyList()
        {
            string location = "Karlsrueh";
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();

            Assert.Throws<ArgumentException>(() => new OpenWeatherQuery(location, specs));
        }


        [Fact]
        public void ReturnTrueWhenAtLeastOneSpecIsSatisfied()
        {
            string location = "Karlsruhe";
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            foreach (int item in new List<int> { 1, 2, 3 })
            {
                specs.Add(CreateSpecification(false));
            }
            specs.Add(CreateSpecification(true));
            Forecast elementToTest = new Mock<Forecast>().Object;

            var QueryUnderTest = new OpenWeatherQuery(location, specs);

            Assert.True(QueryUnderTest.IsSatisfiedBy(elementToTest));
        }

        private ISpecification<Forecast> CreateSpecification(bool returnValue)
        {
            Mock<ISpecification<Forecast>> mockSpec = new Mock<ISpecification<Forecast>>();
            mockSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Forecast>()))
                .Returns(returnValue);
            return mockSpec.Object;
        }

        [Fact]
        public void ReturnFalseWhenAllSpecAreNotSatisfied()
        {
            string location = "Karlsruhe";
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            foreach (int item in new List<int> { 1, 2, 3 })
            {
                specs.Add(CreateSpecification(false));
            }
            Forecast elementToTest = new Mock<Forecast>().Object;

            var QueryUnderTest = new OpenWeatherQuery(location, specs);

            Assert.False(QueryUnderTest.IsSatisfiedBy(elementToTest));
        }

        [Fact]
        public void TakeEnumerableLocationsAndCheckForNullEnumerable()
        {
            List<string> locations = null;
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            specs.Add(CreateSpecification(false));

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherQuery(locations, specs));
        }

        [Fact]
        public void TakeEnumerableLocationsAndCheckForEmptyEnumerable()
        {
            List<string> locations = new List<string>();
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            specs.Add(CreateSpecification(false));

            Assert.Throws<ArgumentException>(() => new OpenWeatherQuery(locations, specs));
        }

        [Fact]
        public void TakeEnumerableLocationsAndCheckForNullElements()
        {
            List<string> locations = new List<string>() { null, "Karlsruhe" };
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            specs.Add(CreateSpecification(false));

            Assert.Throws<ArgumentNullException>(() => new OpenWeatherQuery(locations, specs));
        }

        [Fact]
        public void TakeEnumerableLocationsAndCheckForEmptylElements()
        {
            List<string> locations = new List<string>() { string.Empty, "Karlsruhe" };
            List<ISpecification<Forecast>> specs = new List<ISpecification<Forecast>>();
            specs.Add(CreateSpecification(false));

            Assert.Throws<ArgumentException>(() => new OpenWeatherQuery(locations, specs));
        }
    }
}
