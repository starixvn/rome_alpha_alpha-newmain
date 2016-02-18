namespace Alpha.NewMain.Site.Handlers
{
    using Alpha.NewMain.Site.Models.Requests.CustomerProfile;
    using MediatR;
    using Models.Views.CustomerProfile;
    using Services;

    public class CustomerProfileHandler :
         IRequestHandler<ViewProfileQuery, ViewProfileViewModel>
    {
        private readonly IDashboardService _dashboardService;
        public CustomerProfileHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public ViewProfileViewModel Handle(ViewProfileQuery message)
        {
            return _dashboardService.GetProfile(message);
        }
    }
}