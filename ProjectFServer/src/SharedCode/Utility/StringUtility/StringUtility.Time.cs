using System;

namespace ProjectF
{
    public static partial class StringUtility
    {
        public enum ETimeStringType
        {
            None,

            DaysHoursMinutesSeconds,
            HoursMinutesSeconds,
            MinutesSeconds,
            Seconds,

            TotalHoursAndMinutesSeconds,
            TotalMinutesAndSeconds,

            TotalDays,
            TotalHours,
            TotalMinutes,
            TotalSeconds,
        }

        public static string GetTimeString(TimeSpan timeSpan, ETimeStringType timeStringType) => GetTimeStringInternal(timeStringType, timeSpan.TotalDays, timeSpan.TotalHours, timeSpan.TotalMinutes, timeSpan.TotalSeconds);
        public static string GetTimeString(DateTime dateTime, ETimeStringType timeStringType) => GetTimeStringInternal(timeStringType, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

        private static string GetTimeStringInternal(ETimeStringType timeStringType, double totalDays, double totalHours, double totalMinutes, double totalSeconds)
        {
            double days = Math.Floor(totalDays);
            double hours = Math.Floor(totalHours % 24);
            double minutes = Math.Floor(totalMinutes % 60);
            double seconds = Math.Floor(totalSeconds % 60);

            return timeStringType switch {
                ETimeStringType.None => "",

                ETimeStringType.DaysHoursMinutesSeconds => string.Format("{0}:{1:D2}:{2:D2}:{3:D2}", days, hours, minutes, seconds),
                ETimeStringType.HoursMinutesSeconds => string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds),
                ETimeStringType.MinutesSeconds => string.Format("{0:D2}:{1:D2}", minutes, seconds),
                ETimeStringType.Seconds => string.Format("{0:D2}", seconds),

                ETimeStringType.TotalHoursAndMinutesSeconds => string.Format("{0}:{1:D2}:{2:D2}", totalHours, minutes, seconds),
                ETimeStringType.TotalMinutesAndSeconds => string.Format("{0}:{1:D2}", totalMinutes, seconds),

                ETimeStringType.TotalDays => string.Format("{0}", totalDays),
                ETimeStringType.TotalHours => string.Format("{0}", totalHours),
                ETimeStringType.TotalMinutes => string.Format("{0}", totalMinutes),
                ETimeStringType.TotalSeconds => string.Format("{0}", totalSeconds),

                _ => "InvalidType",
            };
        }
    }
}