using System;
using OpenWeather.Interfaces;
using OpenWeather.Models;
using OpenWeather.Utils;

namespace OpenWeather
{
    public class OpenWeatherTimeSpecification : ISpecification<Forecast>
    {
        private const double _TimeIncludeRange = 1.5;

        public bool IsSatisfiedBy(Forecast element)
        {
            if (element is Forecast forecast)
            {
                var timeDifference = (ComparisonDateTime - TimeZoneInfo.ConvertTimeFromUtc(forecast.MeasureTime, TimeZoneProvider.TimeZone)).TotalHours;
                return (timeDifference <= _TimeIncludeRange && timeDifference > ((-1) * _TimeIncludeRange));
            }
            return false;
        }

        internal DateTime ComparisonDateTime { get; private set; }

        public OpenWeatherTimeSpecification(RelativeDay relativeDay, TimeSpan timeSpan, DateTime referenceDateTime)
        {
            ValidateTimeSpan(timeSpan);

            ComparisonDateTime = new DateTime(
                referenceDateTime.Year,
                referenceDateTime.Month,
                referenceDateTime.Day,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds)
                .AddDays((double)relativeDay);
        }

        private void ValidateTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours >= 24.0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeSpan), ErrorMessages.TimeSpanOutOfRangeExceptionMessage);
            }
        }
    }

    public enum RelativeDay
    {
        Today = 0,
        NextDay = 1,
        DayAfterNextDay = 2
    }
}
