namespace Alpha.Main.Models.Banner
{
    public class BannerItem
    {
        public BannerItem()
        {
        }

        public BannerItem(byte[] imageContent, string mobileBackgroundColor)
        {
            ImageContent = imageContent;
            MobileBackgroundColor = mobileBackgroundColor;
        }

        public byte[] ImageContent { get; }

        public string MobileBackgroundColor { get; }
    }
}
