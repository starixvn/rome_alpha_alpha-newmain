namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using Main.Models.MemberRequests;
    using MediatR;
    using Requests.Dashboard.MemberRequests;
    using ViewModels.Dashboard.MemberRequests;

    public class MemberRequestsHandler : IRequestHandler<MemberRequestsQuery, MemberRequestsViewModel>
    {
        public MemberRequestsHandler()
        {
        }

        public MemberRequestsViewModel Handle(MemberRequestsQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var productRequests = new List<ProductRequest>
            {
                new ProductRequest(5, "Casino", 5),
                new ProductRequest(6, "Fishing World", 15),
                new ProductRequest(7, "Macau Games", 0),
            };

            return new MemberRequestsViewModel(productRequests);
        }
    }
}