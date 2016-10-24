namespace Alpha.Main.Site.Requests.Otp
{
    using Alpha.Main.Site.ViewModels.Security;
    using MediatR;

    public class ResetOtpQuery : IRequest<ResetOtpViewModel>
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }
    }
}