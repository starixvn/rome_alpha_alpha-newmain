namespace Alpha.Main.Helpers.Impl
{
    using System.Web;
    using Fanex.Common.Helper;

    public class SiteMapReaderImpl : SiteMapReader
    {
        public SiteMapNode GetRootNode()
        {
            var rootNode = SiteMap.RootNode;

            Check.Argument.IsNotNull(rootNode, nameof(rootNode));

            return SiteMap.RootNode;
        }
    }
}