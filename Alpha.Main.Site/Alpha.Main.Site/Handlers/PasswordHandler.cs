namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Main.Models;
    using MediatR;
    using Requests.Password;

    public class PasswordHandler :
         IRequestHandler<ChangePasswordCommand, Result>,
         IRequestHandler<ResetMainAccountPasswordCommand, Result>,
         IRequestHandler<ResetSubAccountPasswordCommand, Result>
    {
        public Result Handle(ResetSubAccountPasswordCommand message)
        {
            throw new NotImplementedException();
        }

        public Result Handle(ResetMainAccountPasswordCommand message)
        {
            throw new NotImplementedException();
        }

        public Result Handle(ChangePasswordCommand message)
        {
            throw new NotImplementedException();
        }
    }
}