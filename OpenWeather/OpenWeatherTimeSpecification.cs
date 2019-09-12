using System;
using System.Linq;
using OpenWeather.Interfaces;
using OpenWeather.Models;

namespace OpenWeather
{
    public class OpenWeatherTimeSpecification : ISpecification<Forecast>
    {
        private const string _TimeSpanOutOfRangeExceptionMessage = "The provided timespan must not exceed a total amount of 24 hours. Use relativeDay Argument to achieve this behvaiour.";
        private const double _TimeIncludeRange = 1.5;
        private TimeZoneInfo _EuropeanTimeZone = TimeZoneInfo.GetSystemTimeZones()
            .Where(zone => zone.Id.Contains("W. Europe Standard"))
            .Single();

        public bool IsSatisfiedBy(Forecast element)
        {
            if (element is Forecast forecast)
            {
                var timeDifference = (ComparisonDateTime - TimeZoneInfo.ConvertTimeFromUtc(forecast.MeasureTime, _EuropeanTimeZone)).TotalHours;
                return (timeDifference <= 1.5 && timeDifference > -1.5);
            }
            return false;
        }

        public DateTime ComparisonDateTime { get; private set; }

        public OpenWeatherTimeSpecification(RelativeDay relativeDay, TimeSpan timeSpan, DateTime referenceDateTime)
        {
            ValidateTimeSpan(timeSpan);

            ComparisonDateTime = new DateTime(referenceDateTime.Year,
                referenceDateTime.Month,
                referenceDateTime.Day,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds).AddDays((double)relativeDay);            
        }

        private void ValidateTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours >= 24.0)
            {
                throw new ArgumentOutOfRangeException(_TimeSpanOutOfRangeExceptionMessage, nameof(timeSpan));
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
