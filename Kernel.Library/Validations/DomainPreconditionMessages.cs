namespace Kernel.Library.Validations
{
    using System;

    public static class DomainPreconditionMessages
    {
        public static string GetNotEmpty(string parameterName) => $"{parameterName} can not be empty.";
        public static string GetLongerThan(int maxValueName, string parameterName) => $"{parameterName} length can't be greater than {maxValueName}";
        public static string GetShorterThan(int minValue, string parameterName) => $"{parameterName} can't be smaller than {minValue}";
        public static string GetNotNull(string parameterName) => $"{parameterName} can't be null.";
        public static string GetNotEmptyCollection(string parameterName) => $"At least one {parameterName} has to be set.";
        public static string GetSuccessMatch(string parameterName) => $"{parameterName} must be valid.";
        public static string GetEarlierThan(DateTime dateToCompare, string parameterName) => $"{parameterName} has to be earlier than {dateToCompare}";
        public static string GetEarlierOrEqualThan(DateTime dateToCompare, string parameterName) => $"{parameterName} has to be earlier or equal than {dateToCompare}";
        public static string GetLaterThan(DateTime dateToCompare, string parameterName) => $"{parameterName} has to be later than {dateToCompare}";
        public static string GetLaterOrEqualThan(DateTime dateToCompare, string parameterName) => $"{parameterName} has to be later or equal than {dateToCompare}";
        public static string GreaterThan(int min, string parameterName) => $"{parameterName} has to be greater than {min}";
        public static string LessThanOrEqualTo(int max, string parameterName) => $"{parameterName} has to be less or equal than {max}";
    }
}
