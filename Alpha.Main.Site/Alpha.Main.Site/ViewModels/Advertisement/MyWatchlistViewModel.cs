namespace Alpha.Main.Site.ViewModels.Advertisement
{
    using Alpha.Main.Configurations;
    using Alpha.Web.Helpers;

    public class MyWatchlistViewModel
    {
        public MyWatchlistViewModel(string language, string darkModeName)
        {
            string mainLanguage = Main.Constants.Language.MapToMainLanguageCode(language);
            DesktopImageName = $"Desktop{darkModeName}_{mainLanguage}.jpg";
            MobileImageName = $"Mobile{darkModeName}_{mainLanguage}.jpg";
        }

        public string DesktopImageName { get; }

        public string MobileImageName { get; }

        public string UserGuideLink => AppSettings.IsMBCSite
            ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=179067")
            : MicroSites.MessageUrl("~/StaticMessage/Guide?id=179067");

        public string Version => AppSettings.JsVersion;
    }
}