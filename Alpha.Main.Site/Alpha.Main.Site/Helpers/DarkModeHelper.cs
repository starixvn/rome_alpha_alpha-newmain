namespace Alpha.Main.Site.Helpers
{
    using Sunplus.Agent.Model.Entities;
    using Web.Configurations;

    public static class DarkModeHelper
    {
        public static string GetDarkModeName(IUserSession userSession)
            => AppSettings.IsOfferDarkMode ? userSession.DarkModeName : string.Empty;
    }
}