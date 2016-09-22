namespace Alpha.Main.Site.Requests.Password
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Main.Models;
    using MediatR;

    public class ResetMainAccountPasswordCommand : IRequest<Result>
    {
    }
}