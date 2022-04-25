namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using TelegramBot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Base;

    public class UpdateFunctionCommand : IAsyncRequest<Result>
    {
        public UpdateFunctionCommand() : this(null)
        {
        }

        public UpdateFunctionCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            IP = userSession.UserLoginIp;
        }

        public int UserId { get; set; }

        public short FunctionId { get; set; }

        public string FunctionName { get; set; }

        public byte Type { get; set; }

        public bool Enabled { get; set; }

        public int CustId { get; }

        public string IP { get; }
    }
}