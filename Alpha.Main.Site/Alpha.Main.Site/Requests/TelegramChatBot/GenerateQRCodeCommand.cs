namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using AlphaPlus.User.Models;
    using MediatR;

    public class GenerateQRCodeCommand : IAsyncRequest<VerifyAccountViewModel>
    {
        public GenerateQRCodeCommand() : this(null)
        {
        }

        public GenerateQRCodeCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustomerLevel = userSession.RoleId;
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
        }

        public int CustomerLevel { get; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }
    }
}