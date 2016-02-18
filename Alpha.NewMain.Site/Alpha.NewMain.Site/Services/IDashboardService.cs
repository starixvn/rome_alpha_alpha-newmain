namespace Alpha.NewMain.Site.Services
{
    using System.Collections.Generic;
    using Alpha.NewMain.Site.Models.Dtos;
    using Models.Requests.CustomerProfile;
    using Models.Views.CustomerProfile;

    public interface IDashboardService
    {
        List<LanguageModel> GetLanguages();

        ViewProfileViewModel GetProfile(ViewProfileQuery message);
    }
}
