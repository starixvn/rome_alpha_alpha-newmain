namespace Alpha.Main.Site.ViewModels.Banner
{
    public class ImageRedirectUrlViewModel
    {
        public ImageRedirectUrlViewModel(string imageSource, string redirectUrl = "")
        {
            ImageSource = imageSource;
            RedirectUrl = redirectUrl;
        }

        public string ImageSource { get; }

        public string RedirectUrl { get; }
    }
}