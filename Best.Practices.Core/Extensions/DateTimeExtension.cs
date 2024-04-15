namespace Best.Practices.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime Tomorrow(this DateTime inputDateTime)
        {
            return DateTime.Now.AddDays(1);
        }

        public static DateTime UtcTomorrow(this DateTime inputDateTime)
        {
            return DateTime.UtcNow.AddDays(1);
        }

        public static DateTime UtcYesterday(this DateTime inputDateTime)
        {
            return DateTime.UtcNow.AddDays(-1);
        }

        public static DateTime FirstDayOfMonth(this DateTime inputDateTime)
        {
            var firstDay = new DateTime(inputDateTime.Year, inputDateTime.Month, 1);
            return firstDay;
        }

        public static DateTime LastDayOfMonth(this DateTime inputDateTime)
        {
            var nextMonth = new DateTime(inputDateTime.Year, inputDateTime.Month, 1).AddMonths(1);
            return nextMonth.AddDays(-1);
        }

        public static DateTime LastSecond(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Year, inputDateTime.Month, inputDateTime.Day, 23, 59, 59);
        }

        public static DateTime LastSecondOfMonth(this DateTime inputDateTime)
        {
            var lastDay = inputDateTime.LastDayOfMonth();

            return new DateTime(lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);
        }

        public static bool IsBetweenInclusive(this DateTime inputDateTime, DateTime inputInitialDateTime, DateTime inputFinalDateTime)
        {
            return ((inputDateTime >= inputInitialDateTime) && (inputDateTime <= inputFinalDateTime));
        }

        public static bool IsBetween(this DateTime inputDateTime, DateTime inputInitialDateTime, DateTime inputFinalDateTime)
        {
            return ((inputDateTime > inputInitialDateTime) && (inputDateTime < inputFinalDateTime));
        }

        public static bool IsWeekend(this DateTime inputDateTime)
        {
            return (inputDateTime.DayOfWeek.In(DayOfWeek.Sunday, DayOfWeek.Saturday));
        }

        public static string ToBrazilianFormatWithHour(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("dd/MM/yyyy hh:mm:ss");
        }

        public static string ToBrazilianFormat(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("dd/MM/yyyy");
        }

        public static string ToAmericanFormatWithHour(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("yyyy/MM/dd hh:mm:ss");
        }

        public static string ToAmericanFormat(this DateTime inputDateTime)
        {
            return inputDateTime.ToString("yyyy/MM/dd");
        }
    }
}