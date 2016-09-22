namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Requests.Home;
    using Services;
    using System;
    using ViewModels.Home.Index;

    public class HomeHandler : IRequestHandler<BalanceInfoQuery, BalanceInfoViewModel>
    {
        private readonly HomeInfoService homeInfoService;

        public HomeHandler(HomeInfoService homeInfoService)
        {
            this.homeInfoService = homeInfoService;
        }

        public BalanceInfoViewModel Handle(BalanceInfoQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var balanceInfo = homeInfoService.GetBalanceInfo(message.CurrentUser);

            var viewModel = new BalanceInfoViewModel(message.CurrentUser, balanceInfo);

            return viewModel;
        }
    }
}