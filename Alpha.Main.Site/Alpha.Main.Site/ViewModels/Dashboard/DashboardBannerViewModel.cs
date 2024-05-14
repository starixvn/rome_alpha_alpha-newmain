namespace Alpha.Main.Site.ViewModels.Dashboard
{
    using Alpha.Main.Configurations;
    using AlphaPlus.User.Models;

    public class DashboardBannerViewModel
    {
        public DashboardBannerViewModel(IUserSession userSession)
        {
            IsShow = AppSettings.DashboardBannerOfferedSites.Contains("*")
                || AppSettings.DashboardBannerOfferedSites.Contains(userSession.Site);
            if (IsShow)
            {
                SetBannerUrls(userSession);
            }
        }

        public bool IsShow { get; private set; }

        public string BannerName { get; private set; }

        public string BannerMobileName { get; private set; }

        private void SetBannerUrls(IUserSession userSession)
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var language = Main.Constants.Language.MapToMainLanguageCode(userSession.Language);
            var version = AppSettings.JsVersion;
            BannerName = $"{folder}/banner_{language}.jpg?v={version}";
            BannerMobileName = $"{folder}/banner_mobile_{language}.jpg?v={version}";
        }
    }
}