namespace Alpha.Main.Site.Handlers
{
    using System;
    using MediatR;
    using Requests.BalanceInfo;
    using Services;
    using ViewModels.Dashboard;

    public class BalanceInfoHandler : IRequestHandler<BalanceInfoQuery, BalanceInfoViewModel>
    {
        private BalanceInfoService balanceInfoService;

        public BalanceInfoHandler(
            BalanceInfoService balanceInfoService)
        {
            this.balanceInfoService = balanceInfoService;
        }

        public BalanceInfoViewModel Handle(BalanceInfoQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var balanceInfo = balanceInfoService.GetBalanceInfo(message.CustId, message.UserLevel);

            var viewModel = new BalanceInfoViewModel(balanceInfo, message.UserLevel, message.Currency);

            return viewModel;
        }
    }
}