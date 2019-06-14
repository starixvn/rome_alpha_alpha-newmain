namespace Alpha.Main.Criteria
{
    using Fanex.Data.Repository;
    using Params;

    public class KickoutCustomerCriteria : NonQueryCommand
    {
        public KickoutCustomerCriteria(KickoutCustomerParam param)
        {
            CustId = param.CustId;
            RoleId = param.CustLevel.Value;
            UplineUsername = param.UplineUsername;
            ServerId = param.ServerId;
            IP = param.IP;
            SubAccId = param.SubAccountId;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public string UplineUsername { get; }

        public int ServerId { get; }

        public string IP { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_OnUser_ForceKickedout";

        public override bool IsValid()
            => CustId > 0 && RoleId > 0 && !string.IsNullOrWhiteSpace(UplineUsername) && ServerId > 0;
    }
}
