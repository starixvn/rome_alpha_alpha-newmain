namespace Alpha.Main.Services
{
    using Models;
    using Params.Password;

    public interface PasswordService
    {
        Result ResetMainAccountPassword(ResetMainAccountPasswordParam param);

        Result ChangeMainAccountPassword(ChangeMainAccountPasswordParam param);

        Result ResetSubAccountPassword(ResetSubAccountPasswordParam param);

        Result ChangeSubAccountPassword(ChangeSubAccountPasswordParam param);

        Result SkipForceChangePassword(SkipForceChangePasswordParam param);
    }
}
