namespace Alpha.Main.Site.ViewModels.Banner
{
    public class BannerViewModel
    {
        public BannerViewModel()
            : this(string.Empty, string.Empty)
        {
        }

        public BannerViewModel(string imageContent, string mobileBackgroundColor)
        {
            ImageContent = imageContent;
            MobileBackgroundColor = mobileBackgroundColor;
        }

        public string ImageContent { get; }

        public string MobileBackgroundColor { get; }
    }
}