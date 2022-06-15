namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using TelegramBot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Base;

    public class UpdateLanguageCommand : IAsyncRequest<Result>
    {
        public UpdateLanguageCommand() : this(null)
        {
        }

        public UpdateLanguageCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            IP = userSession.UserLoginIp;
        }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public string IP { get; }
    }
}