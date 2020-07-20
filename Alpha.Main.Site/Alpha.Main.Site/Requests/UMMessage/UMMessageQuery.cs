namespace Alpha.Main.Site.Requests.UMMessage
{
    using System.Collections.Generic;
    using Alpha.Main.Models.UMMessage;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class UMMessageQuery : IAsyncRequest<IEnumerable<MessageDTO>>
    {
        private readonly IUserSession userSession;

        public UMMessageQuery()
            : this(new UserSessionExt())
        {
        }

        public UMMessageQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId => userSession.CustId;

        public string Language => userSession.Language;
    }
}