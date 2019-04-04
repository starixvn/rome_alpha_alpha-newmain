namespace Alpha.Main.Site.Requests
{
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class KickoutQuery : IRequest<bool>
    {
        public KickoutQuery(IUserSession userSession)
        {
            OnlineUserId = userSession.OnUserId;
            ActorName = userSession.UserName;
            ActorLevelId = userSession.RoleId;
            SubUserName = userSession.SubUserName;
            NickName = userSession.NickName;
            ServerId = userSession.ServerId;
        }

        public int OnlineUserId { get; }

        public string ActorName { get; }

        public byte ActorLevelId { get; }

        public string SubUserName { get; }

        public string NickName { get; }

        public int ServerId { get; }
    }
}