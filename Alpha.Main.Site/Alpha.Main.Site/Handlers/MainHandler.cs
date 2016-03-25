namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Models.Dtos;
    using Models.Requests;
    using Models.Views;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
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

        private MenuBarViewModel LeftMenu()
        {
            
            var menus = _dashboardService.GetLeftMenu();
            var pageAccess = UserSession.Current.SubPageAccess;

            //var leftMenu = new MenuBarViewModel
            //{
            //    IsMainMenuVisible = _dashboardService.IsShowLeftMenu(),
            //    IsAccountInfoVisible = pageAccess.ViewMemberInfo,
            //    MenuItems = menus
            //};

            //var leftMenu = new MenuBarViewModel(_dashboardService.IsShowLeftMenu(), pageAccess.ViewMemberInfo, menus);

            throw new System.NotImplementedException();
        }
    }
}