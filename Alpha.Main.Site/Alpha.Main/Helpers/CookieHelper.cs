namespace Alpha.Main.Helpers
{
    using System;

    public static class CookieHelper
    {
        public static string GetCurrentUrlCookieName(
            int custId,
            int subAccId,
            Func<string, string> encodeDataFunction)
        {
            var joinStrings = new string[]
            {
                custId.ToString(),
                subAccId.ToString(),
                "NewMain"
            };

            var cacheUrlCookieName = string.Join("_", joinStrings);

            return encodeDataFunction?.Invoke(cacheUrlCookieName) ?? cacheUrlCookieName;
        }
    }
}