#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.BalanceInfo
{
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard;

    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        private readonly IUserSession userSession;

        public BalanceInfoQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            }
        }

        public string Currency
        {
            get
            {
                return this.userSession.Currency;
            }
        }
    }
}