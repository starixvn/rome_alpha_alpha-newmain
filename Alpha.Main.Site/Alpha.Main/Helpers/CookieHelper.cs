namespace Alpha.Main.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class CookieHelper
    {
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
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