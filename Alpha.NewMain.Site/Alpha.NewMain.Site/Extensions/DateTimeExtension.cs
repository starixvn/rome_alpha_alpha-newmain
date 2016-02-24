namespace Alpha.NewMain.Site.Extensions
{
    using System;
    using Alpha.NewMain.Site.Constants;

    public static class DateTimeExtension
    {
        public static string ToShortFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(DateTimeFormatConstant.ShortDateWithSlash);
        }
    }
}