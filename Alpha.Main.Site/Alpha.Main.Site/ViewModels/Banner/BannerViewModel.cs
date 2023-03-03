namespace Alpha.Main.Site.ViewModels.Banner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Main.Configurations;
    using Alpha.Main.Constants;

    public class BannerViewModel
    {
        public BannerViewModel(byte type, string language, string imageContent)
        {
            Type = type;
            Language = language;
            Banners = GetBanners(imageContent).ToArray();
        }

        public byte Type { get; }

        public string Language { get; }

        public ImageRedirectUrlViewModel[] Banners { get; }

        private IEnumerable<ImageRedirectUrlViewModel> GetBanners(string imageContent)
        {
            if (!string.IsNullOrWhiteSpace(imageContent))
            {
                yield return new ImageRedirectUrlViewModel(imageContent);
            }

            if (!AppSettings.BannerIsHideMobileSabaApp)
            {
                yield return new ImageRedirectUrlViewModel(
                    GetMobileSabaAppImageSource(AppSettings.SiteLabel),
                    $"https://www.winwin999.net/?bt={GetMobileSabaAppToken(AppSettings.SiteLabel)}&pd=sports");
            }

            if (!AppSettings.IsMBCSite)
            {
                yield return new ImageRedirectUrlViewModel(
                    GetSabaSportMarketingAppImageSource(),
                    "https://www.sabavn.com/");
            }
        }

        private static string GetMobileSabaAppToken(string siteLabel)
        {
            if (siteLabel.Equals("Nova88", StringComparison.OrdinalIgnoreCase))
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

        private string MapSabaSportMarketingAppLanguage()
            => Language.Equals(Main.Constants.Language.Vietnamese, StringComparison.OrdinalIgnoreCase)
            ? Main.Constants.Language.Vietnamese
            : Main.Constants.Language.English;
    }
}