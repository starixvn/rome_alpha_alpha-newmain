namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class AddAccountCommand : WriteLogCommandBase, IAsyncRequest<AddingUserResult>
    {
        public AddAccountCommand()
            : this(null)
        {
        }

        internal AddAccountCommand(IUserSession userSession)
            : base(userSession)
        {
        }

        public int CustLevel => userSession.RoleId;

        public int SubAccountId => userSession.SubAccId;

        public string Language => userSession.Language;

        public string LetstalkId { get; set; }
    }
}