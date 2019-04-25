namespace Alpha.Main.Site.Handlers
{
    using Main.Models;
    using MediatR;
    using Params.Nickname;
    using Requests.Nickname;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class NicknameHandler : IRequestHandler<CreateNicknameCommand, Result>
    {
        private readonly NicknameService nicknameService;
        private readonly IUserSession userSession;

        public NicknameHandler(
            NicknameService nicknameService,
            IUserSession userSession)
        {
            this.nicknameService = nicknameService;
            this.userSession = userSession;
        }

        public Result Handle(CreateNicknameCommand message)
        {
            var isEnableUpdateNickname = !message.IsDisableUpdateNickname;
            var nickname = message.Nickname == null ? string.Empty : message.Nickname.Trim();

            var result = nicknameService.CreateNickname(
                new CreateNicknameParam(message.ActorId, nickname, message.IP, message.LanguageKey, isEnableUpdateNickname));

            if (result.IsSuccessful)
            {
                userSession.IsNicknameLoginOnly = true;

                if (isEnableUpdateNickname)
                {
                    userSession.NickName = nickname;
                }
            }

            return result;
        }
    }
}