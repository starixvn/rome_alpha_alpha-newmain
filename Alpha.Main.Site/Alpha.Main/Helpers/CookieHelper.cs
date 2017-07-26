namespace Alpha.Main.Helpers
{
    using System;

    public static class CookieHelper
    {
        public static string GetCookieNameOfCurrentPage(
            int custId,
            int subAccId,
            Func<string, string> encodeDataFunction)
        {
            var joinStrings = new[]
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
