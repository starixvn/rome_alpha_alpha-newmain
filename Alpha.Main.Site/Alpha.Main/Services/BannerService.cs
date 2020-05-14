namespace Alpha.Main.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Models.Banner;

    public interface BannerService
    {
        Task<BannerItem> GetBanner(string languageCode, byte type);
    }
}
