namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Alpha.Main.Models.Banner;
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

            return new BannerViewModel(message.Type, message.Language, ToBase64String(banner));
        }

        private static string ToBase64String(BannerItem banner)
            => banner == null || banner.ImageContent == null || banner.ImageContent.Length == 0
            ? string.Empty
            : $"data:image/png;base64,{Convert.ToBase64String(banner.ImageContent)}";
    }
}