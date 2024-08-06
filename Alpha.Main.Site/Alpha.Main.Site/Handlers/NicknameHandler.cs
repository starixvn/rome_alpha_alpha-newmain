namespace Alpha.Main.Site.Handlers
{
    using AlphaPlus.User.Models;
    using Main.Models;
    using MediatR;
    using Params.Nickname;
    using Requests.Nickname;
    using Services;
    using System.Threading.Tasks;

    public class NicknameHandler : IAsyncRequestHandler<CreateNicknameCommand, Result>
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

        public async Task<Result> Handle(CreateNicknameCommand message)
        {
            var isEnableUpdateNickname = !message.IsDisableUpdateNickname;
            var nickname = message.Nickname == null ? string.Empty : message.Nickname.Trim();
            var isNicknameLoginOnly = true;

            var result = await nicknameService.CreateNickname(
                new CreateNicknameParam(
                    message.ActorId,
                    message.SubAccountId,
                    nickname,
                    message.IP,
                    message.Language,
                    message.Site,
                    isEnableUpdateNickname),
                isNicknameLoginOnly);

            if (result.IsSuccessful)
            {
                userSession.IsNicknameLoginOnly = isNicknameLoginOnly;

                if (isEnableUpdateNickname)
                {
                    userSession.NickName = nickname.ToUpperInvariant();
                }
            }

            return result;
        }
    }
}