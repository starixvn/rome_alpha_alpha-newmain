namespace Alpha.Main.Site.Requests.Password
{
    using MediatR;
    using ViewModels.Security;

    public class ChangePasswordQuery : IRequest<PasswordViewModel>
    {
    }
}