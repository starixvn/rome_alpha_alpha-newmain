namespace Alpha.Main.Services
{
    using Models;
    using Models.Otp;
    using Params.Otp;

    public interface CustomerOtpService
    {
        Result VerifyOtp(CustomerOtp param);

        Result SetUpOtp(CustomerOtp param);

        Result RemoveSecretKey(RemoveSecretKeyParam param);

        Result ChangeOtpStatus(ChangeOtpStatusParam param);

        CustomerOtp GenerateCustomerOtp(int custId, string username, int subAccId = 0);

        CustomerOtp GetCustomerOtp(int custId, int subAccId = 0);

        bool IsEnabledOtp(int custId, int subAccId = 0);

        bool IsDifferenceDevice(int custId, string deviceId, int subAccId = 0);
    }
}