namespace Alpha.Main.Site.DataAccess.Impl
{
    using System.Web;

    public class HomeDataAccessImpl: HomeDataAccess
    {
        public SiteMapNode GetSiteMapNode()
        {
            var node = SiteMap.Provider.RootNode;
            return node;
        }
    }
}