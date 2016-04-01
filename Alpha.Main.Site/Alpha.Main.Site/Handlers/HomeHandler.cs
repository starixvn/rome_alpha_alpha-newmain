namespace Alpha.Main.Site.Handlers
{
    using System.Web;
    using MediatR;
    using Models.Dtos;
    using Models.Requests;
    using Models.Views;
    using Services;

    public class HomeHandler :
        IRequestHandler<MainIndexQuery, MainIndexViewModel>,
        IRequestHandler<ChangeLanguageQuery, ResultModel>
    {
        private readonly IDashboardService _dashboardService;

        public HomeHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public MainIndexViewModel Handle(MainIndexQuery message)
        {
            var result = new MainIndexViewModel();

            _dashboardService.StoreSession(result);
            return result;
        }

        public ResultModel Handle(ChangeLanguageQuery message)
        {
            return _dashboardService.ChangeLanguage(message);
        }
    }
}