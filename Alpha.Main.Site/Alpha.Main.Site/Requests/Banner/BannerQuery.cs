namespace Alpha.Main.Site.Requests.Banner
{
    using Alpha.Main.Site.ViewModels.Banner;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class BannerQuery : IAsyncRequest<BannerViewModel>
    {
        private readonly IUserSession userSession;

        public BannerQuery() : this(null)
        {
        }

        public BannerQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public string Language => userSession.Language;

        public byte Type { get; set; }

        public bool IsProtectedMode => userSession.IsProtectedMode;
    }
}