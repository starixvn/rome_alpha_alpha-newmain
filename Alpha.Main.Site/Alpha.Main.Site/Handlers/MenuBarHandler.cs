namespace Alpha.Main.Site.Handlers
{
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
            var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel);

            var viewModel = new MenuBarViewModel(menuItems);

            return viewModel;
        }
    }
}