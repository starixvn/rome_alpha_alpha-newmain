namespace Alpha.Main.Site.Requests.Nickname
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class CreateNicknameCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public CreateNicknameCommand(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public string Nickname { get; set; }

        public int ActorId => userSession.CustId;

        public string IP => userSession.UserLoginIp;

        public string LanguageKey => userSession.LangKey;

        public bool IsValid()
            => !userSession.IsSubAcc
            && !userSession.IsInternal
            && (string.IsNullOrWhiteSpace(userSession.NickName) || !userSession.IsNicknameLoginOnly);
    }
}