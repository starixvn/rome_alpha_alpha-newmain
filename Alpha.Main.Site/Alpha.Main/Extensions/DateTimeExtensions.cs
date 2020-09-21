namespace Alpha.Main.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string LongDateWithSlash = "dd/MM/yyyy hh:mm:ss tt";

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);
    }
}
