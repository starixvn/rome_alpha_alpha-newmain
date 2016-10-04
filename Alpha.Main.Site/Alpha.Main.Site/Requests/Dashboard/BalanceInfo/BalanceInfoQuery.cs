#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.BalanceInfo
{
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard;

    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        public BalanceInfoQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            Currency = userSession.Currency;
        }

        public int CustId { get; private set; }

        public UserLevel UserLevel { get; private set; }

        public string Currency { get; private set; }
    }
}