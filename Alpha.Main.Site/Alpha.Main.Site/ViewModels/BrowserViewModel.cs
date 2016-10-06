namespace Alpha.Main.Site.ViewModels
{
    using Main.Models.Enumerations;

    public class BrowserViewModel
    {
        public bool IsUnsupportedBrowser { get; set; }

        public string DownloadNewBrowserUrl { get; set; }

        public UserLevel UserLevel { get; set; }
    }
}