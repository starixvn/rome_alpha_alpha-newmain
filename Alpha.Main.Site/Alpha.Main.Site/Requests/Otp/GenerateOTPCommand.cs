using Alpha.Main.Models;
using AlphaPlus.User.Models;
using MediatR;

namespace Alpha.Main.Site.Requests.Otp
{
    public class GenerateOTPCommand : IAsyncRequest<Result>
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