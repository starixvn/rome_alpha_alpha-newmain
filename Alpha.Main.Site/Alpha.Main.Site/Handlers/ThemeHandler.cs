namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.Theme;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Web.Configurations;

    public class ThemeHandler :
        IRequestHandler<ChangeThemeQuery, bool>,
        IRequestHandler<ResetAssetAppPathQuery, bool>
    {
        private readonly IUserSession userSession;
        private readonly ThemeService themeService;

        public ThemeHandler(IUserSession userSession, ThemeService themeService)
        {
            this.userSession = userSession;
            this.themeService = themeService;
        }

        public bool Handle(ChangeThemeQuery message)
        {
            if (AppSettings.IsOfferDarkMode)
            {
                userSession.DarkModeName = message.IsDarkMode ? "-dark" : string.Empty;

                if (!message.IsInternal)
                {
                    themeService.WriteLog(
                        message.CustId,
                        message.SubAccId,
                        message.IsDarkMode,
                        message.IP,
                        message.Location);
                }

                return true;
            }

            return false;
        }

        public bool Handle(ResetAssetAppPathQuery message)
        {
            userSession.AssetAppPath = AppSettings.AssetAppPath;

            return true;
        }
    }
}