namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;
    using Alpha.Main.Site.Models.Dtos;
    using Models.Menus;
    using Models.Requests;
    using Models.Views;

    public interface IDashboardService
    {
        List<string> GetHotMessages();

        ResultModel ChangeLanguage(ChangeLanguageQuery message);

        string GetCookieCurrentURL();

        string GetBaseUrl();

        string GetMainPage();

        bool IsMainPageFromCookie();
        
        void StoreSession(MainIndexViewModel dashboardViewModel);
        
        bool IsShowLeftMenu();
    }
}
