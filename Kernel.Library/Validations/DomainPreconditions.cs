namespace Kernel.Library.Validations
{
    using Kernel.Library.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class DomainPreconditions
    {
        public static void NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(DomainPreconditionMessages.GetNotEmpty(parameterName));
            }
        }

        public static void NotEmpty(byte[] value, string parameterName)
        {
            if (value.Length <= 0)
            {
                throw new DomainException(DomainPreconditionMessages.GetNotEmpty(parameterName));
            }
        }

        public static void LongerThan(string value, int maxValueName, string parameterName)
        {
            if (value?.Length > maxValueName)
            {
                throw new DomainException(DomainPreconditionMessages.GetLongerThan(maxValueName, parameterName));
            }
        }

        public static void LongerThan(int value, int maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                throw new DomainException(DomainPreconditionMessages.GetLongerThan(maxValue, parameterName));
            }
        }

        public static void ShorterThan(int value, int minValue, string parameterName)
        {
            if (minValue > value)
            {
                throw new DomainException(DomainPreconditionMessages.GetShorterThan(minValue, parameterName));
            }
        }

        internal static void ShorterThan(decimal? value, int minValue, string parameterName)
        {
            if (value.HasValue && value.Value < minValue)
            {
                throw new DomainException(DomainPreconditionMessages.GetShorterThan(minValue, parameterName));
            }
        }

        public static void RegexMatch(string value, string regexPattern, string parameterName)
        {
            var regex = new Regex(regexPattern);
            var match = regex.Match(value);
            if (!match.Success)
            {
                throw new DomainException(DomainPreconditionMessages.GetSuccessMatch(parameterName));
            }
        }

        public static T NotNull<T>(T value, string parameterName)
            where T : class
        {
            return value ?? throw new DomainException(DomainPreconditionMessages.GetNotNull(parameterName)); ;
        }

        public static T IsNull<T>(T value, string message)
            where T : class
        {
            if (value != null)
            {
                throw new DomainException(message);
            }

            return value;
        }

        public static IEnumerable<T> NotEmpty<T>(IEnumerable<T> values, string parameterName) where T : class
        {
            if (values != null && !values.Any())
            {
                throw new DomainException(DomainPreconditionMessages.GetNotEmptyCollection(parameterName));
            }

            return values;
        }

        public static void EarlierThan(DateTime? value, DateTime dateToCompare, string parameterName)
        {
            if (value.HasValue && value.Value >= dateToCompare)
            {
                throw new DomainException(DomainPreconditionMessages.GetEarlierThan(dateToCompare, parameterName));
            }
        }

        public static void EarlierOrEqualThan(DateTime start, DateTime end, string parameterName)
        {
            if (start > end)
            {
                throw new DomainException(DomainPreconditionMessages.GetEarlierOrEqualThan(end, parameterName));
            }
        }

        public static void LaterThan(DateTime? value, DateTime dateToCompare, string parameterName)
        {
            if (value.HasValue && value.Value <= dateToCompare)
            {
                throw new DomainException(DomainPreconditionMessages.GetLaterThan(dateToCompare, parameterName));
            }
        }

        public static void LaterOrEqualThan(DateTime? value, DateTime dateToCompare, string parameterName)
        {
            if (value.HasValue && value.Value < dateToCompare)
            {
                throw new DomainException(DomainPreconditionMessages.GetLaterOrEqualThan(dateToCompare, parameterName));
            }
        }

        public static void GreaterThan(int quantity, int min, string parameterName)
        {
            if (quantity <= min)
            {
                throw new DomainException(DomainPreconditionMessages.GreaterThan(min, parameterName));
            }
        }

        public static void LessThanOrEqualTo(int quantity, int max, string parameterName)
        {
            if (quantity > max)
            {
                throw new DomainException(DomainPreconditionMessages.LessThanOrEqualTo(max, parameterName));
            }
        }
    }
}
