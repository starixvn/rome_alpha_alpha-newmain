namespace Alpha.NewMain.Site.Extensions
{
    using System;

    public static class NumberExtension
    {
        public static string Format(this int value)
        {
            return DoFormat(value, 0);
        }

        public static string Format(this object value)
        {
            return DoFormat(value, 0);
        }

        public static string Format(this decimal value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        public static string Format(this object value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        public static string Format(this double value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        public static string Format(this float value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        private static string DoFormat(object value, byte decDigits)
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }

            string formatString = "{0:n" + decDigits.ToString() + "}";
            return string.Format(formatString, value);
        }
    }
}
