#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Dashboard.BalanceInfo
{
    using System;
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.BalanceInfo;

    public class BalanceInfoQuery : IRequest<BalanceInfoViewModel>
    {
        public BalanceInfoQuery(IUserSession userSession, Func<int, bool> canViewProduct)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            Currency = userSession.Currency;
            CanViewReports = userSession.SubPageAccess.ViewReports;
            CanViewProduct = canViewProduct;
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }

        public string Currency { get; }

        public bool CanViewReports { get; }

        public Func<int, bool> CanViewProduct { get; }
    }
}