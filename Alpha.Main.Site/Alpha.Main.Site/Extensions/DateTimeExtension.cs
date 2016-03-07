namespace Alpha.Main.Site.Extensions
{
    using System;
    using Alpha.Main.Site.Constants;

    public static class DateTimeExtension
    {
        public static string ToShortFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(DateTimeFormatConstant.ShortDateWithSlash);
        }

        public static string ToLongFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(DateTimeFormatConstant.LongDateWithSlash);
        }

        public static string ToShortLongFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(DateTimeFormatConstant.ShortLongDateWithSlash);
        }
    }
}