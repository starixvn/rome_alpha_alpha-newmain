namespace Alpha.Main.Site.ViewModels.Banner
{
    public class ImageRedirectUrlViewModel
    {
        public ImageRedirectUrlViewModel(string imageSource, string redirectUrl, bool logClick, string target)
        {
            ImageSource = imageSource;
            RedirectUrl = redirectUrl;
            LogClick = logClick;
            Target = target;
        }

        public string ImageSource { get; }

        public string RedirectUrl { get; }

        public bool LogClick { get; }

        public string Target { get; }
    }
}