using System;

namespace Orchard.Park.Core
{
    public static class DateHelper
    {
        /// <summary>
        /// 计算两个日期间隔
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public static TimeSpan Diff(this DateTime dateStart, DateTime dateEnd)
        {
            var sp = dateEnd.Subtract(dateStart);

            return sp;
        }

        /// <summary>
        /// 时间转换到00:00:00
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime ToStartTime(this DateTime datetime)
        {
            return Convert.ToDateTime(datetime.ToString("D"));
        }

        /// <summary>
        /// 时间转换到23:59:59
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime ToEndTime(this DateTime datetime)
        {
            return Convert.ToDateTime(datetime.AddDays(1).ToString("D")).AddSeconds(-1);
        }

        /// <summary>
        /// 时间转换到HH:mm:00
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime ToMinutes(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);
        }
    }
}