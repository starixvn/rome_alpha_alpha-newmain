namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;
    using Alpha.Main.Site.Models.Dtos;
    using Models.Requests;
    using Models.Requests.CustomerProfile;
    using Models.Views;

    public interface IDashboardService
    {
        List<LanguageModel> GetLanguages();

        List<string> GetMonthNames();

        List<string> GetHotMessages();

        ProfilePictureModel GetProfile(ViewProfileQuery message);

        ResultModel ChangeLanguage(ChangeLanguageQuery message);

        string GetCookieCurrentURL();

        string GetBaseUrl();

        string GetMainPage();

        bool IsMainPageFromCookie();

        List<MenuItem> GetLeftMenu();

        void StoreSession(MainIndexViewModel dashboardViewModel);

        string GetPasswordHash();

        bool IsShowLeftMenu();
    }
}
