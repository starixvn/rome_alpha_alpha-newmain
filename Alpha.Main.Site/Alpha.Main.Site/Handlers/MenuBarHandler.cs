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
            var menuItems = siteMenuService.GetMenuByUser(message.User);

            var viewModel = new MenuBarViewModel(menuItems);

            return viewModel;
        }
    }
}