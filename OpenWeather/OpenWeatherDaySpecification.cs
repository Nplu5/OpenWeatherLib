using System;
using System.Collections.Generic;
using System.Text;
using OpenWeather.Interfaces;
using OpenWeather.Models;
using OpenWeather.Utils;

namespace OpenWeather
{
    public class OpenWeatherDaySpecification : ISpecification<Forecast>
    {
        internal DateTime ComparisonDateTime { get; private set; }
        public OpenWeatherDaySpecification(RelativeDay day, DateTime reference)
        {
            ComparisonDateTime = reference.AddDays((double) day);
        }
        public bool IsSatisfiedBy(Forecast element)
        {
            if (element is Forecast forecast)
            {
                return DateTime.Equals(ComparisonDateTime.Date, TimeZoneInfo.ConvertTimeFromUtc(forecast.MeasureTime, TimeZoneProvider.TimeZone).Date);
            }

            return false;
        }
    }
}
