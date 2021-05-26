namespace Alpha.Main.Params
{
    using Alpha.Auth.Configurations;
    using Alpha.Web.Models;

    public class KickoutCustomerParam
    {
        public KickoutCustomerParam(
            int custId,
            UserLevel custLevel,
            int serverId,
            string ip,
            int subAccountId = 0)
        {
            CustId = custId;
            CustLevel = custLevel;
            ServerId = serverId;
            IP = ip;
            SubAccountId = subAccountId;
            IsDisabledRecheckin = AppSettings.IsDisabledCheckDuplicationLogin;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public int ServerId { get; }

        public string IP { get; }

        public int SubAccountId { get; }

        public bool IsDisabledRecheckin { get; }
    }
}
