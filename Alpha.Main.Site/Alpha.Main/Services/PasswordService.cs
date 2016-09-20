namespace Alpha.Main.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Params.Password;

    public interface PasswordService
    {
        Result ResetMainAccountPassword(ResetMainAccountPasswordParam param);

        Result ChangeMainAccountPassword(UpdateMainAccountPasswordParam param);

        Result ResetSubAccountPassword(ResetSubAccountPasswordParam param);

        Result ChangeSubAccountPassword(UpdateSubAccountPasswordParam param);
    }
}
