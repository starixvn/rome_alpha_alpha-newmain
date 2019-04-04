namespace Alpha.Main.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params;

    public class LogoutCommand : NonQueryCommand
    {
        public LogoutCommand(LogoutParam logoutParam) : this(
            logoutParam.OnlineUserId,
            string.IsNullOrWhiteSpace(logoutParam.SubUserName) ? logoutParam.CustName : logoutParam.SubUserName,
            logoutParam.CustRoleId,
            logoutParam.ServerId,
            logoutParam.NickName,
            logoutParam.Reason)
        {
        }

        public LogoutCommand(
            int onlineUserId,
            string userName,
            byte roleId,
            int serverId,
            string nickname,
            string reason)
        {
            OnlineUserId = onlineUserId;
            UserName = userName;
            RoleId = roleId;
            ServerId = serverId;
            Nickname = nickname;
            Reason = reason;
        }

        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; }

        public string UserName { get; }

        public byte RoleId { get; }

        public int ServerId { get; }

        public string Nickname { get; }

        public string Reason { get; }

        public override string GetSettingKey()
            => "Age_CheckOut";

        public override bool IsValid()
            => OnlineUserId > 0
            && !string.IsNullOrWhiteSpace(UserName)
            && RoleId > 0
            && ServerId > 0;
    }
}
