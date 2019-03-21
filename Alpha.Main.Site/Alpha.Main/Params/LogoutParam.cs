namespace Alpha.Main.Params
{
    public class LogoutParam
    {
        public LogoutParam(
            int onlineUserId,
            string custName,
            string subUserName,
            string nickName,
            byte custRoleId,
            int serverId)
        {
            OnlineUserId = onlineUserId;
            CustName = custName;
            SubUserName = subUserName;
            NickName = nickName;
            CustRoleId = custRoleId;
            ServerId = serverId;
        }

        public int OnlineUserId { get; }

        public string CustName { get; }

        public string SubUserName { get; }

        public string NickName { get; }

        public byte CustRoleId { get; }

        public int ServerId { get; }
    }
}
