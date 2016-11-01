namespace Alpha.Main.Site.Helpers
{
    using Main.Models.Otp;
    using ViewModels;

    public interface BrowserHelper
    {
        bool IsMobile();

        BrowserViewModel CheckSupportedBrowsers();

        DeviceInfo GetDeviceInfo();
    }
}