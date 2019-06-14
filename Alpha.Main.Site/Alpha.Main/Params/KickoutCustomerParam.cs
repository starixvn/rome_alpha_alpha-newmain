namespace Alpha.Main.Params
{
    using Models.Enumerations;

    public class KickoutCustomerParam
    {
        public KickoutCustomerParam(
            int custId,
            UserLevel custLevel,
            string uplineUsername,
            int serverId,
            string ip,
            int subAccountId = 0)
        {
            CustId = custId;
            CustLevel = custLevel;
            UplineUsername = uplineUsername;
            ServerId = serverId;
            IP = ip;
            SubAccountId = subAccountId;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public string UplineUsername { get; }

        public int ServerId { get; }

        public string IP { get; }

        public int SubAccountId { get; }
    }
}
