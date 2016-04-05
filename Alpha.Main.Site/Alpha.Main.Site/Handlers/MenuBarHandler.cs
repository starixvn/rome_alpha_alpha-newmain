namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Models.Requests;
    using Services;
    using ViewModels;

    public class MenuBarHandler :
        IAsyncRequestHandler<MenuBarQuery, MenuBarViewModel>
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

        Task<MenuBarViewModel> IAsyncRequestHandler<MenuBarQuery, MenuBarViewModel>.Handle(MenuBarQuery message)
        {
            throw new NotImplementedException();
        }
    }
}