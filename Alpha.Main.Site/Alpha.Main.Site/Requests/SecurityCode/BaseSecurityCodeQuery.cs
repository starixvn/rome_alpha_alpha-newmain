namespace Alpha.Main.Site.Models.Request.SecurityCode
{
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Security;
    public class BaseSecurityCodeQuery : IRequest<BaseSecurityCodeViewModel>
    {
        private readonly IUserSession userSession;
        public BaseSecurityCodeQuery(IUserSession userSession)
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

        public int SubAccId
        {
            get
            {
                return this.userSession.SubAccId;
            }
        }


        public bool IsSubAcc
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }
    }
}