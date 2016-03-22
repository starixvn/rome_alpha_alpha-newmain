namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Models.Dtos;
    using Models.Requests;
    using Models.Views;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class MainHandler :
        IRequestHandler<MainIndexQuery, MainIndexViewModel>,
        IRequestHandler<ChangeLanguageQuery, ResultModel>
    {
        private readonly IDashboardService _dashboardService;

        public MainHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public MainIndexViewModel Handle(MainIndexQuery message)
        {
            var result = new MainIndexViewModel
            {
                Languages = _dashboardService.GetLanguages(),
                MonthNames = _dashboardService.GetMonthNames(),
                HotMessages = _dashboardService.GetHotMessages(),
                CookieCurrentURLKey = _dashboardService.GetCookieCurrentURL(),
                BaseUrl = _dashboardService.GetBaseUrl(),
                MainPage = _dashboardService.GetMainPage(),
                IsMainPageFromCookie = _dashboardService.IsMainPageFromCookie(),
                SideBar = LeftMenu(),
                PasswordHash = _dashboardService.GetPasswordHash()
            };

            _dashboardService.StoreSession(result);
            return result;
        }

        public ResultModel Handle(ChangeLanguageQuery message)
        {
            return _dashboardService.ChangeLanguage(message);
        }

        private NavigationViewModel LeftMenu()
        {
            var menus = _dashboardService.GetLeftMenu();
            var pageAccess = UserSession.Current.SubPageAccess;
            var leftMenu = new NavigationViewModel
            {
                IsShowMenu = _dashboardService.IsShowLeftMenu(),
                ShowAccountInfo = pageAccess.ViewMemberInfo,
                MenuList = menus
            };

            return leftMenu;
        }
    }
}