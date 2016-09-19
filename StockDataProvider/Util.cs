using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DataProvider
{
    public static class Util
    {
        //EST Time
        //static TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time, TimeZoneInfo tzi)
        {          
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0),tzi);
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000/1000;            //除10000调整为13位
            return t;
        }
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d, TimeZoneInfo tzi)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1),tzi);
            time = startTime.AddSeconds(d);
            return time;
        }

        static  readonly TimeZoneInfo utcZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
        static readonly TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
        public static DateTime ConvertFromUtcIntToEst(uint utcInt)
        {
            return ConvertDateTimeFromUTC2EST(ConvertIntDateTime(utcInt, utcZone));
        }

        public static DateTime ConvertDateTimeFromEST2UTC(DateTime dt)
        {
            //TimeZoneInfo source = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
           return  ConvertDateTimeToUtc(dt, "US Eastern Standard Time");
        }
        public static DateTime ConvertDateTimeFromUTC2EST(DateTime dt)
        {
            //TimeZoneInfo source = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
            return ConvertDateTimeFromUtc(dt, "US Eastern Standard Time");
        }
        public static DateTime ConvertDateTimeToUtc(DateTime dt, string source)
        {
            var timeZoneSource = TimeZoneInfo.FindSystemTimeZoneById(source);
            var timeZoneDestination = TimeZoneInfo.FindSystemTimeZoneById("UTC");       
            return TimeZoneInfo.ConvertTime(dt, timeZoneSource, timeZoneDestination);
        }
        public static DateTime ConvertDateTimeFromUtc(DateTime dt, string targe)
        {
            var timeZoneSource = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            var timeZoneDestination = TimeZoneInfo.FindSystemTimeZoneById(targe);
            return TimeZoneInfo.ConvertTime(dt, timeZoneSource, timeZoneDestination);
        }

        public static DateTime ConvertDateTimeZoneByZoneId(DateTime dt, String source, String targe)
        {
            var timeZoneSource = TimeZoneInfo.FindSystemTimeZoneById(source);
           
            var timeZoneDestination = TimeZoneInfo.FindSystemTimeZoneById(targe);
            return TimeZoneInfo.ConvertTime(dt, timeZoneSource, timeZoneDestination);
        }
      
    }
}
