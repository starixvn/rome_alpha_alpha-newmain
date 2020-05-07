namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.Banner;
    using Alpha.Main.Site.ViewModels.Banner;
    using MediatR;

    public class BannerHandler : IAsyncRequestHandler<BannerQuery, BannerViewModel>
    {
        private readonly BannerService bannerService;

        public BannerHandler(BannerService bannerService)
        {
            this.bannerService = bannerService;
        }

        public async Task<BannerViewModel> Handle(BannerQuery message)
        {
            var banner = await bannerService.GetBanner(message.Language, message.Type);

            return banner == null
                ? new BannerViewModel()
                : new BannerViewModel(ToBase64String(banner.ImageContent), banner.MobileBackgroundColor);
        }

        private static string ToBase64String(byte[] data)
            => data == null || data.Length == 0
            ? string.Empty
            : $"data:image/png;base64,{Convert.ToBase64String(data)}";
    }
}