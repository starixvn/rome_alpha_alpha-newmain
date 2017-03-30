namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Linq;
    using Constants;
    using MediatR;
    using Requests.Dashboard.MemberRequests;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.MemberRequests;

    public class MemberRequestsHandler : IRequestHandler<MemberRequestsQuery, MemberRequestsViewModel>
    {
        private readonly MemberRequestsService service;
        private readonly IUserSession userSession;

        public MemberRequestsHandler(
            MemberRequestsService service,
            IUserSession userSession)
        {
            this.service = service;
            this.userSession = userSession;
        }

        public MemberRequestsViewModel Handle(MemberRequestsQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var productRequests = service.GetProductRequestData(message.CustId)
                                        .Where(product => IsShowProduct(product.ProductId))
                                        .Select(product => product);
         
            var viewModel = new MemberRequestsViewModel(
                productRequests, 
                message.UserLevel, 
                message.UserSession.SubPageAccess.ViewReports,
                message.UserSession.SubPageAccess.ViewMemberInfo);
            return viewModel;
        }

        public bool IsShowProduct(int productId) 
            => productId != ProductId.FishingWorld || !userSession.GetHideProduct(productId);
    }
}