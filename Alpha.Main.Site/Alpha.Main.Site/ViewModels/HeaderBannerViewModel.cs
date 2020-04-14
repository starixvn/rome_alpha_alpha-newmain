namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Configurations;

    public class HeaderBannerViewModel
    {
        private const string Mobile = "/Mobile";
        private const string RootBannerURL = "~/Assets/Images/HeaderBanner";

        private readonly string language;
        private readonly bool isMobile;

        public HeaderBannerViewModel(string language, bool isMobile = false)
        {
            this.language = language;
            this.isMobile = isMobile;
        }

        public string GetRandomImageFileName()
        {
            if (AppSettings.HideHeaderBanner)
            {
                return string.Empty;
            }

            var imageFileNames = GetImageFileNames();

            if (imageFileNames.Count == 0)
            {
                return string.Empty;
            }

            var randomImageFileName = GetRandomImageFileName(imageFileNames);

            return $"{MapRootBannerURLToLangugeFolder()}/{randomImageFileName}";
        }

        private IList<string> GetImageFileNames()
            => Directory
                .GetFiles(HttpContext.Current.Server.MapPath(MapRootBannerURLToLangugeFolder()))
                .Select(Path.GetFileName)
                .ToList();

        private string MapRootBannerURLToLangugeFolder()
        {
            var currentLanguage = language;

            if (currentLanguage.Equals("ja-JP", StringComparison.OrdinalIgnoreCase)
                || currentLanguage.Equals("ko-KR", StringComparison.OrdinalIgnoreCase)
                || currentLanguage.Equals("th-TH", StringComparison.OrdinalIgnoreCase))
            {
                currentLanguage = "en-US";
            }

            return $"{RootBannerURL}/{currentLanguage}{(isMobile ? Mobile : string.Empty)}";
        }

        private static string GetRandomImageFileName(IList<string> imageFileNames)
        {
            var random = new Random();

            return imageFileNames[random.Next(0, imageFileNames.Count)];
        }   
    }
}