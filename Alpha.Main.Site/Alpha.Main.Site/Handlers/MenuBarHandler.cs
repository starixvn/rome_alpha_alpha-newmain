namespace Alpha.Main.Site.Handlers
{
    using System;
    using MediatR;
    using Models.Requests;
    using Services;
    using ViewModels;

    public class MenuBarHandler :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly SiteMenuService siteMenuService;

        public MenuBarHandler(
            SiteMenuService siteMenuService)
        {
            this.siteMenuService = siteMenuService;
        }

        public MenuBarViewModel Handle(MenuBarQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel, message.IsCookieLess);

            var viewModel = new MenuBarViewModel(
                menuItems,
                message.IsSmallDevice,
                isDarkMode: !string.IsNullOrWhiteSpace(message.DarkModeName),
                isHideBanner: message.IsHideBanner);

            return viewModel;
        }
    }
}