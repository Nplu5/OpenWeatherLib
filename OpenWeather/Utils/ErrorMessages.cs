using System.Runtime.CompilerServices;

namespace OpenWeather.Utils
{
    internal class ErrorMessages
    {
        internal static string LocationFormatError { get; } = "The location must be of the format of: {town},{countrycode}";
        internal static string QueryMustContainAtLeastOneElementError { get; } = "Query must at least contain one condition";
        internal static string LocationMustNotBeEmptyError { get; } = "Location paramter must not be empty";
        internal static string TimeSpanOutOfRangeExceptionMessage { get; } = "The provided timespan must not exceed a total amount of 24 hours. Use relativeDay Argument to achieve this behvaiour";

        internal static string ArgumentNullMessage(string parameterName, [CallerMemberName] string callingMethod = "")
        {
            return $"{parameterName} must not be null when passed into {callingMethod}";
        }
    }
}
