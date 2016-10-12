namespace Alpha.Main.Helpers.Impl
{
    using System.Web;
    using Fanex.Common.Helper;

    public class SiteMapReaderImpl : SiteMapReader
    {
        public SiteMapNode GetRootNode(SiteMapNode rootNode = null)
        {
            var results = rootNode ?? SiteMap.RootNode;

            Check.Argument.IsNotNull(results, nameof(results));

            return results;
        }
    }
}