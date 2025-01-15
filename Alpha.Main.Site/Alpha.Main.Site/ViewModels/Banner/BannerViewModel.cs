namespace Alpha.Main.Site.ViewModels.Banner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Main.Configurations;
    using Alpha.Main.Constants;
    using Alpha.Main.Site.Helpers;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;

    public class BannerViewModel
    {
        public BannerViewModel(byte type, string language, string imageContent, IUserSession userSession)
        {
            Type = type;
            Language = language;
            UserSession = userSession;
            Banners = GetBanners(imageContent).ToArray();
        }

        public byte Type { get; }

        public string Language { get; }

        public IUserSession UserSession { get; }

        public ImageRedirectUrlViewModel[] Banners { get; }

        private IEnumerable<ImageRedirectUrlViewModel> GetBanners(string imageContent)
        {
            var userLevelName = Enumeration.FromValue<UserLevel>(UserSession.RoleId).DisplayName;
            if (AppSettings.IsOfferedWhiteLabel(UserSession.Site, userLevelName))
            {
                var url = UrlHelper.MapMainPath("~/Dashboard/WhiteLabel");
                yield return new ImageRedirectUrlViewModel(GetS1AppImageSource(), url, true, "main");
            }

            if (!string.IsNullOrWhiteSpace(imageContent))
            {
                yield return new ImageRedirectUrlViewModel(imageContent, string.Empty, false, null);
            }

            if (!AppSettings.BannerIsHideMobileSabaApp)
            {
                yield return new ImageRedirectUrlViewModel(
                    GetMobileSabaAppImageSource(AppSettings.SiteLabel),
                    $"https://www.winwin999.net/?bt={GetMobileSabaAppToken(AppSettings.SiteLabel)}&pd=sports",
                    false,
                    null);
            }

            if (!AppSettings.IsMBCSite)
            {
                yield return new ImageRedirectUrlViewModel(GetSabaSportMarketingAppImageSource(), "https://www.sabavn.com/", false, null);
            }
        }

        private static string GetMobileSabaAppToken(string siteLabel)
        {
            if (siteLabel.Equals("MBC", StringComparison.OrdinalIgnoreCase))
            {
                return "GueGhmPRxEWbbp3FIbpqPA";
            }

            if (siteLabel.Equals("IBC", StringComparison.OrdinalIgnoreCase))
            {
                return "QTQanXruEEur8ihzdYoF3A";
            }

            if (siteLabel.Equals(AppSettings.SiteLabel789y, StringComparison.OrdinalIgnoreCase))
            {
                return "Rl89LflI4EaoNIdu2zzeVw";
            }

            return string.Empty;
        }

        private string GetMobileSabaAppImageSource(string siteGroup)
            => string.Format(
                "~/Assets/Images/MenuBarBanner/{0}/{1}_msa_{2}.jpg",
                siteGroup,
                Type == BannerType.Mobile ? "600x140" : "280x70",
                Main.Constants.Language.MapToMainLanguageCode(Language));

        private string GetSabaSportMarketingAppImageSource()
            => string.Format(
                "~/Assets/Images/MenuBarBanner/789Y/{0}_ssma_{1}.jpg",
                Type == BannerType.Mobile ? "600x140" : "280x70",
                MapSabaSportMarketingAppLanguage());

        private string GetS1AppImageSource()
            => string.Format(
                "~/Assets/Images/MenuBarBanner/789Y/{0}_s1msa_{1}.jpg",
                Type == BannerType.Mobile ? "600x140" : "280x70",
                Language);

        private string MapSabaSportMarketingAppLanguage()
            => Language.Equals(Main.Constants.Language.Vietnamese, StringComparison.OrdinalIgnoreCase)
            ? Main.Constants.Language.Vietnamese
            : Main.Constants.Language.English;
    }
}