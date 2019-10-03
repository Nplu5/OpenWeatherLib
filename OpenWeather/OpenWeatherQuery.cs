using System;
using System.Collections.Generic;
using System.Linq;
using OpenWeather.Interfaces;
using OpenWeather.Models;
using OpenWeather.Utils;

namespace OpenWeather
{
    public class OpenWeatherQuery : IQuery<string, Forecast>
    {
        #region ctors
        public OpenWeatherQuery(string location, ISpecification<Forecast> spec)
        {
            ValidateLocation(location);
            ValidateSpec(spec);

            Queries = new List<string>() { location };
            Specifications = new List<ISpecification<Forecast>>() { spec };
        }
        public OpenWeatherQuery(string location, IEnumerable<ISpecification<Forecast>> specs)
        {
            ValidateLocation(location);
            ValidateEnumerableInput(specs, ValidateSpec);
            Queries = new List<string>() { location };
            Specifications = specs;
        }
        public OpenWeatherQuery(IEnumerable<string> locations, IEnumerable<ISpecification<Forecast>> specs)
        {
            ValidateEnumerableInput(specs, ValidateSpec);
            ValidateEnumerableInput(locations, ValidateLocation);
            Queries = locations;
            Specifications = specs;
        }
        public OpenWeatherQuery(IEnumerable<string> locations, ISpecification<Forecast> spec)
        {
            ValidateEnumerableInput(locations, ValidateLocation);
            ValidateSpec(spec);
            Queries = locations;
            Specifications = new List<ISpecification<Forecast>>() { spec };
        }
        #endregion

        #region input Validation functions
        private void ValidateEnumerableInput<T>(IEnumerable<T> enumerable, Action<T> validateElementAction)
        {
            if (enumerable is null)
                throw new ArgumentNullException(ErrorMessages.ArgumentNullMessage(nameof(enumerable)));

            if (!enumerable.Any())
                throw new ArgumentException(ErrorMessages.QueryMustContainAtLeastOneElementError);

            foreach (var item in enumerable)
                validateElementAction(item);
        }

        private static void ValidateSpec(ISpecification<Forecast> spec)
        {
            if (spec is null)
                throw new ArgumentNullException(ErrorMessages.ArgumentNullMessage(nameof(spec)));
        }

        private static void ValidateLocation(string location)
        {
            if (location is null)
                throw new ArgumentNullException(ErrorMessages.ArgumentNullMessage(nameof(location)));

            if (string.IsNullOrEmpty(location))
                throw new ArgumentException(ErrorMessages.LocationMustNotBeEmptyError);
        }
        #endregion

        public IEnumerable<ISpecification<Forecast>> Specifications { get; }
        public IEnumerable<string> Queries { get; }


        public bool IsSatisfiedBy(Forecast element)
        {
            return Specifications.Any(spec => spec.IsSatisfiedBy(element));
        }
    }
}
