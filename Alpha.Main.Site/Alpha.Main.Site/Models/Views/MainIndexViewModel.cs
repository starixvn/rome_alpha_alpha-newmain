namespace Alpha.Main.Site.Models.Views
{
    using System.Collections.Generic;
    using ViewModels;

    public class MainIndexViewModel
    {
        public string CookieCurrentURLKey { get; set; }

        public string BaseUrl { get; set; }

        public string MainPage { get; set; }

        public string PasswordHash { get; set; }

        public bool IsMainPageFromCookie { get; set; }

        public MenuBarViewModel SideBar { get; set; }
    }
}