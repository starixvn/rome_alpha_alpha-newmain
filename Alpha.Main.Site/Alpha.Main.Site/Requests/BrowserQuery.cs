namespace Alpha.Main.Site.Requests
{
    using Alpha.Main.Site.ViewModels;
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class BrowserQuery : IRequest<BrowserViewModel>
    {
        private readonly IUserSession userSession;
        public BrowserQuery(IUserSession userSession)
        {
            if (userSession.RoleId == 0)
            {
                WriteLog(userSession);
                this.userSession = UserSession.Current;
            }
            else
            {
                this.userSession = userSession;
            }
        }

        public void WriteLog(IUserSession userSession)
        {
            Logs.WriteInfo(string.Join(
                BuildLog("UserId", userSession.UserId.ToString(), UserSession.Current.CustId.ToString()),
                BuildLog("Username", userSession.UserName, UserSession.Current.UserName),
                BuildLog("NetSessionId", userSession.NetSessionId, UserSession.Current.NetSessionId)
                ));
        }

        private string BuildLog(string name, string fromUserSession, string fromUserSessionCurrent)
        {
            return $"{name}: userSession:{fromUserSession}, UserSession.Current:{fromUserSessionCurrent}\r\n";
        }

        public byte UserLevelId
        {
            get
            {
                return this.userSession.RoleId;
            }
        }
    }
}