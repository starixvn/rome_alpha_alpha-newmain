namespace Alpha.Main.Site.Handlers
{
    using System;
    using MediatR;
    using Requests.Dashboard.BalanceInfo;
    using Services;
    using ViewModels.Dashboard.BalanceInfo;

    public class BalanceInfoHandler : IRequestHandler<BalanceInfoQuery, BalanceInfoViewModel>
    {
        private readonly BalanceInfoService balanceInfoService;

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

            var viewModel = new BalanceInfoViewModel(
                balanceInfo,
                message.UserLevel,
                message.Currency,
                message.CustId);

            return viewModel;
        }
    }
}