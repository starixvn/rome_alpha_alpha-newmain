namespace Alpha.Main.Site.Requests.Home
{
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Home.Index;

    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        private readonly IUserSession userSession;

        public BalanceInfoQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public IUserSession CurrentUser { get { return this.userSession; } }
    }
}