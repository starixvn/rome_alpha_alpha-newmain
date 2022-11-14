
namespace Alpha.Main.Site.Requests.ApiKey
{
    using Alpha.Main.Models.ApiKey;
    using Alpha.Main.Site.ViewModels.ApiKey;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class RevokeApiKeyCommand : IAsyncRequest<Result>
    {
        public RevokeApiKeyCommand() : this(null)
        {
        }

        public RevokeApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
        }

        public string Key { get; set; }
    }
}