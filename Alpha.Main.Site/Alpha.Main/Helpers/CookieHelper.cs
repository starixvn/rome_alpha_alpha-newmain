namespace Alpha.Main.Helpers
{
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public static class CookieHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Reviewed!")]
        public static string GetCurrentUrlCookieName()
        {
            var joinStrings = new string[]
                    {
                       UserSession.Current.CustId.ToString(),
                       UserSession.Current.SubAccId.ToString(),
                       "NewMain"
                    };
            var cacheUrlCookieName = string.Join("_", joinStrings);

            return Encryption.PearsonEncode(cacheUrlCookieName);
        }
    }
}