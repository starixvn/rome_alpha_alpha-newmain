namespace Alpha.Main.Site.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Main.Models.Enumerations;

    public class BrowserViewModel
    {
        public bool IsUnsupportedBrowser { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed!")]
        public string DownloadNewBrowserUrl { get; set; }

        public UserLevel UserLevel { get; set; }
    }
}