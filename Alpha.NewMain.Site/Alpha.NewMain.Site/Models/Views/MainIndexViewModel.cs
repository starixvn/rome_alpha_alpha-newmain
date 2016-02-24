namespace Alpha.NewMain.Site.Models.Views
{
    using System.Collections.Generic;
    using Dtos;

    public class MainIndexViewModel
    {
        public List<LanguageModel> Languages { get; set; }

        public List<string> MonthNames { get; set; }

        public List<string> HotMessages { get; set; }

        public string CookieCurrentURLKey { get; set; }

        public string BaseUrl { get; set; }

        public string MainPage { get; set; }

        public string PasswordHash { get; set; }

        public bool IsMainPageFromCookie { get; set; }

        public LeftMenuViewModel LeftMenu { get; set; }
    }
}