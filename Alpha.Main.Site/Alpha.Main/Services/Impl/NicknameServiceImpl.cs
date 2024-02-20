namespace Alpha.Main.Services.Impl
{
    using System.Text.RegularExpressions;
    using AppResources;
    using Criteria.Nickname;
    using Fanex.Data.Repository;
    using Models;
    using Params.Nickname;

    public class NicknameServiceImpl : NicknameService
    {
        private const int Code = 100;
        private static readonly Regex NicknameRule = new Regex(@"^[a-zA-Z0-9]{5,20}$");

        private readonly IDynamicRepository dynamicRepository;

        public NicknameServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Result CreateNickname(CreateNicknameParam param, bool nickNameLoginOnly)
        {
            if (param.IsEnableUpdateNickname)
            {
                var result = ValidateAndCreateNickname(param);

                if (result.IsFailed)
                {
                    return result;
                }
            }

            return UpdateNicknameLoginOnly(
                new UpdateNicknameLoginOnlyParam(param, nickNameLoginOnly));
        }

        private static bool IsValidNickName(string nickName)
            => NicknameRule.IsMatch(nickName);

        private Result ValidateAndCreateNickname(CreateNicknameParam param)
        {
            if (IsValidNickName(param.Nickname))
            {
                NicknameNonQueryCommand command = param.IsSubAccount
                    ? new CreateSubAccountNicknameCommand(param.CustId, param.SubAccountId, param.Nickname, param.LanguageKey, param.IP)
                    : new CreateNicknameCommand(param.CustId, param.Nickname, param.LanguageKey, param.IP);

                return Execute(command);
            }

            return Result.CreateErrorResult(Resources.NicknameRule, Code);
        }

        private Result UpdateNicknameLoginOnly(UpdateNicknameLoginOnlyParam param)
        {
            NicknameNonQueryCommand command = param.IsSubAccount
                ? new UpdateSubAccountNicknameLoginOnlyCommand(param.CustId, param.SubAccountId, param.NickNameLoginOnly, param.LanguageKey, param.IP)
                : new UpdateNicknameLoginOnlyCommand(param.CustId, param.NickNameLoginOnly, param.LanguageKey, param.IP);

            return Execute(command);
        }

        private Result Execute(NicknameNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return Result.CreateErrorResult(
                command.ErrCode == 0 ? Resources.ConfiguredNicknameSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
