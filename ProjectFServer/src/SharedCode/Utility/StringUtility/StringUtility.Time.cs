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

            FlexibleHMS,
            Flexiblehms,
        }

        public static string GetTimeString(ETimeStringType timeStringType, TimeSpan timeSpan) => GetTimeString(timeStringType, timeSpan.TotalDays, timeSpan.TotalHours, timeSpan.TotalMinutes, timeSpan.TotalSeconds);
        public static string GetTimeString(ETimeStringType timeStringType, DateTime dateTime) => GetTimeString(timeStringType, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        public static string GetTimeString(ETimeStringType timeStringType, double totalDays, double totalHours, double totalMinutes, double totalSeconds)
        {
            int days = (int)Math.Floor(totalDays);
            int hours = (int)Math.Floor(totalHours % 24);
            int minutes = (int)Math.Floor(totalMinutes % 60);
            int seconds = (int)Math.Floor(totalSeconds % 60);

            return timeStringType switch {
                ETimeStringType.None => "",

                ETimeStringType.DaysHoursMinutesSeconds => string.Format("{0}:{1:D2}:{2:D2}:{3:D2}", days, hours, minutes, seconds),
                ETimeStringType.HoursMinutesSeconds => string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds),
                ETimeStringType.MinutesSeconds => string.Format("{0:D2}:{1:D2}", minutes, seconds),
                ETimeStringType.Seconds => string.Format("{0:D2}", seconds),

                ETimeStringType.TotalHoursAndMinutesSeconds => string.Format("{0:F0}:{1:D2}:{2:D2}", totalHours, minutes, seconds),
                ETimeStringType.TotalMinutesAndSeconds => string.Format("{0:F0}:{1:D2}", totalMinutes, seconds),

                ETimeStringType.TotalDays => string.Format("{0:F0}", totalDays),
                ETimeStringType.TotalHours => string.Format("{0:F0}", totalHours),
                ETimeStringType.TotalMinutes => string.Format("{0:F0}", totalMinutes),
                ETimeStringType.TotalSeconds => string.Format("{0:F0}", totalSeconds),

                ETimeStringType.FlexibleHMS => GetUpperFlexibleTimeString(hours, minutes, seconds),
                ETimeStringType.Flexiblehms => GetLowerFlexibleTimeString(hours, minutes, seconds),

                _ => "InvalidType",
            };
        }

        private static string GetUpperFlexibleTimeString(int hours, int minutes, int seconds)
        {
            if(hours > 0)
                return string.Format("{0:F0}H {1:D2}M", hours, minutes);
            else if(minutes > 0)
                return string.Format("{0:F0}M {1:D2}S", minutes, seconds);
            else
                return string.Format("{0:F0}S", seconds);
        }

        private static string GetLowerFlexibleTimeString(int hours, int minutes, int seconds)
        {
            if(hours > 0)
                return string.Format("{0:F0}h {1:D2}m", hours, minutes);
            else if(minutes > 0)
                return string.Format("{0:F0}m {1:D2}s", minutes, seconds);
            else
                return string.Format("{0:F0}s", seconds);
        }
    }
}