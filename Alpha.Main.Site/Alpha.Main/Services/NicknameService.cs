namespace Alpha.Main.Services
{
    using Models;
    using Params.Nickname;
    using System.Threading.Tasks;

    public interface NicknameService
    {
        Task<Result> CreateNickname(CreateNicknameParam param, bool nickNameLoginOnly);
    }
}
