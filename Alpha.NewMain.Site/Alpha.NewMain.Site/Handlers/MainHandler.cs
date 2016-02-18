namespace Alpha.NewMain.Site.Handlers
{
    using MediatR;
    using Models.Requests;
    using Models.Views;
    using Services;

    public class MainHandler :
        IRequestHandler<MainIndexQuery, MainIndexViewModel>
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
                Languages = _dashboardService.GetLanguages()
            };

            return result;
        }
    }
}