namespace Alpha.Main.Site.Requests.Banner
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.ViewModels.Banner;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class BannerQuery : IAsyncRequest<BannerViewModel>
    {
        public BannerQuery() : this(null)
        {
        }

        internal BannerQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            Language = userSession.Language;
            IsHideBanner = AppSettings.BannerIsHide || userSession.IsProtectedMode;
        }

        public string Language { get; }

        public bool IsHideBanner { get; }

        public byte Type { get; set; }
    }
}