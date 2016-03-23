namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests.Home;
    using MediatR;
    using Models.Views;
    using Services;

    public class HomeHandler :
          IRequestHandler<NavigationQuery, NavigationViewModel>
    {
        private readonly HomeService homeService;

        private readonly IDashboardService dashboardService;

        public HomeHandler(IDashboardService dashboardService, HomeService homeService)
        {
            this.dashboardService = dashboardService;
            this.homeService = homeService;
        }

        public NavigationViewModel Handle(NavigationQuery query)
        {
            var menus = homeService.GetNavigation(query);
            var leftMenu = new NavigationViewModel
            {
                IsShowMenu = dashboardService.IsShowLeftMenu(),
                ShowAccountInfo = query.SubPageAccess.ViewMemberInfo,
                MenuList = menus
            };

            return leftMenu;
        }
    }
}