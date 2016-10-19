namespace Alpha.Main.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;

    public interface OtpService
    {
        Result VerifyOtp();

        Result SetUpOtp();

        Result ChangeOtpStatus();
    }
}
