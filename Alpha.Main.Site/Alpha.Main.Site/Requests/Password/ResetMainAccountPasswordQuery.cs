namespace Alpha.Main.Site.Requests.Password
{
    using Alpha.Main.Site.ViewModels.Security;
    using Alpha.Web.Models;
    using MediatR;

    public class ResetMainAccountPasswordQuery : IRequest<PasswordViewModel>
    {
        public int CustId { get; set; }

        public byte CustRoleId { get; set; }

        public string Username { get; set; }

        public bool IsValid() => UserLevel.Member.Value <= CustRoleId && CustRoleId <= UserLevel.Super.Value;
    }
}