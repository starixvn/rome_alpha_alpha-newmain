namespace Alpha.Main.Models.Banner
{
    public class BannerItem
    {
        public BannerItem()
        {
        }

        public BannerItem(byte[] imageContent)
        {
            ImageContent = imageContent;
        }

        public byte[] ImageContent { get; }
    }
}
