namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests.Home;
    using MediatR;
    using Models.Views;
    using Services;

    public class HomeHandler :
          IRequestHandler<NavigationQuery, NavigationViewModel>
    {
        private readonly SiteMenuService siteMenuService;
        private readonly IDashboardService dashboardService;

        public HomeHandler(IDashboardService dashboardService, SiteMenuService siteMenuService)
        {
            this.dashboardService = dashboardService;
            this.siteMenuService = siteMenuService;
        }

        public NavigationViewModel Handle(NavigationQuery query)
        {
            var menus = siteMenuService.GetMenuByUser(query.GetUser());

            var model = new NavigationViewModel
            {
                IsMainMenuVisible = dashboardService.IsShowLeftMenu(),
                IsAccountInfoVisible = query.SubPageAccess.ViewMemberInfo,
                MenuItems = menus
            };

            return model;
        }
    }
}