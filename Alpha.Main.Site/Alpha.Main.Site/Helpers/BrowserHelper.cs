namespace Alpha.Main.Site.Helpers
{
    using ViewModels;

    public interface BrowserHelper
    {
        bool IsMobile();

        BrowserViewModel CheckSupportedBrowsers();
    }
}