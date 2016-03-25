namespace Alpha.Main.Site.Repositories
{
    using System.Collections.Generic;
    using System.Web;
    using Criteria.CustomerProfile;
    using Models.Dtos;

    public interface IDashboardRepository
    {
        List<LanguageModel> GetLanguages();

        SiteMapNode GetSiteMapNode();
    }
}