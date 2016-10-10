namespace Alpha.Main.Site.Requests.Password
{
    using MediatR;
    using ViewModels.Security;

    public class ResetSubAccountPasswordQuery : IRequest<PasswordViewModel>
    {
        public int SubAccountId { get; set; }

        public string Username { get; set; }
    }
}