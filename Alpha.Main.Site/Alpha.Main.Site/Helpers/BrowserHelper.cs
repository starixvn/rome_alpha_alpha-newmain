namespace Alpha.Main.Site.Helpers
{
    using Main.Models.Otp;
    using ViewModels;

    public interface BrowserHelper
    {
        bool IsMobile(bool isOnlyTablet);

        BrowserViewModel CheckSupportedBrowsers();

        Device GetDevice();

        string GetDeviceId();

        string GetUserAgent();
    }
}