namespace Alpha.Main.Site.Requests.ChatBot
{
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class WriteLogCommandBase
    {
        protected readonly IUserSession userSession;

        protected WriteLogCommandBase(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public string IP => userSession.UserLoginIp;
    }
}