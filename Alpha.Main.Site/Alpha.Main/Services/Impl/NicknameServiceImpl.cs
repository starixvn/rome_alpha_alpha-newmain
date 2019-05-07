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
        private static readonly Regex NicknameRule = new Regex(@"^[a-zA-Z0-9]{5,15}$");

        private readonly IDynamicRepository dynamicRepository;

        public NicknameServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Result CreateNickname(CreateNicknameParam param)
        {
            if (param.IsEnableUpdateNickname)
            {
                var result = IsValidNickName(param.Nickname)
                    ? Execute(new CreateNicknameCommand(param))
                    : Result.CreateErrorResult(Resources.NicknameRule, 100);

                if (result.IsFailed)
                {
                    return result;
                }
            }

            return UpdateNicknameLoginOnly(
                new UpdateNicknameLoginOnlyParam(param.CustId, true, param.IP, param.LanguageKey));
        }
        

        private static bool IsValidNickName(string nickName)
            => NicknameRule.IsMatch(nickName);

        private Result UpdateNicknameLoginOnly(UpdateNicknameLoginOnlyParam param)
            => Execute(new UpdateNicknameLoginOnlyCommand(param));

        private Result Execute(NicknameNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return Result.CreateErrorResult(
                command.ErrCode == 0 ? Resources.ConfiguredNicknameSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
