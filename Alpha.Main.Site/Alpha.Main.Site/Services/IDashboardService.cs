namespace Alpha.Main.Site.Services
{
    using Alpha.Main.Site.Models.Dtos;
    using Models.Requests;
    using Models.Views;
    using System.Collections.Generic;

    public interface IDashboardService
    {
        ResultModel ChangeLanguage(ChangeLanguageQuery message);

        void StoreSession(MainIndexViewModel dashboardViewModel);
    }
}
