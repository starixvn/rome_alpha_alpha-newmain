using MediatR;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Requests.Otp
{
    public class GenerateOTPCommand : IAsyncRequest<long>
    {
        public GenerateOTPCommand(IUserSession userSession)
        {
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }
    }
}