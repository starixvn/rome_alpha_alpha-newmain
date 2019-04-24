namespace Alpha.Main.Site.ViewModels.Nickname
{
    using Helpers;
    using Main.Models;
    using Sunplus.Agent.Model.Entities;

    public class CreateNicknameViewModel
    {
        private readonly IUserSession userSession;

        public CreateNicknameViewModel(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public Result Result { get; set; }

        public static string MainRootPath
            => UrlHelper.MapMainPath("~");

        public bool IsValid()
            => !userSession.IsSubAcc
            && !userSession.IsInternal
            && (string.IsNullOrWhiteSpace(userSession.NickName) || !userSession.IsNicknameLoginOnly);
    }
}