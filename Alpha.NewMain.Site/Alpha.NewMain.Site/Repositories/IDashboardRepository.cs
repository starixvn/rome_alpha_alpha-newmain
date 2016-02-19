namespace Alpha.NewMain.Site.Repositories
{
    using System.Collections.Generic;
    using System.Web;
    using Criteria.CustomerProfile;
    using Models.Dtos;
    using Models.Views.CustomerProfile;

    public interface IDashboardRepository
    {
        List<LanguageModel> GetLanguages();

        ProfilePictureModel GetAvatar(ViewProfileCriteria criteria);

        SiteMapNode GetSiteMapNode();
    }
}