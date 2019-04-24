namespace Alpha.Main.Services
{
    using Models;
    using Params.Nickname;

    public interface NicknameService
    {
        Result CreateNickname(CreateNicknameParam param);

        Result UpdateNicknameLoginOnly(UpdateNicknameLoginOnlyParam param);
    }
}
