namespace Alpha.NewMain.Site.Services
{
    using System.Collections.Generic;
    using Alpha.NewMain.Site.Models.Dtos;
    using Models.Requests;
    using Models.Requests.CustomerProfile;
    using Models.Views.CustomerProfile;

    public interface IDashboardService
    {
        List<LanguageModel> GetLanguages();

        List<string> GetMonthNames();

        List<string> GetHotMessages();

        ViewProfileViewModel GetProfile(ViewProfileQuery message);

        ResultModel ChangeLanguage(ChangeLanguageQuery message);

        string GetCookieCurrentURL();

        string GetBaseUrl();

        string GetMainPage();

        bool IsMainPageFromCookie();
    }
}
