namespace Alpha.Main.Site.Requests.Password
{
    using Alpha.Main.Site.ViewModels.Security;
    using MediatR;

    public class ResetMainAccountPasswordQuery : IRequest<PasswordViewModel>
    {
        public int CustId { get; set; }

        public byte CustRoleId { get; set; }

        public bool IsCasinoSync { get; set; }
    }
}