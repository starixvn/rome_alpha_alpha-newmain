namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.Theme;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ThemeHandler : IRequestHandler<ChangeThemeQuery, bool>
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
            if (AppSettings.IsMBCSite)
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
    }
}