namespace Alpha.Main.Services
{
    using Models;
    using Params.SecurityCode;

    public interface SecurityCodeService
    {
        Result ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param);

        Result ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param);

        Result ResetSubAccountSecurityCode(ResetSecurityCodeParam param);

        string GenerateSecurityToken();

        string GenerateSalt(int custId);

        Result VerifySecurityCode(VerifySecurityCodeParam param);

        Result UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param);
    }
}
