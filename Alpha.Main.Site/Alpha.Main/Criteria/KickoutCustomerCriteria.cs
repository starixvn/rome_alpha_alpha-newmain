namespace Alpha.Main.Criteria
{
    using Fanex.Data.Repository;

    public class KickoutCustomerCriteria : NonQueryCommand
    {
        public KickoutCustomerCriteria(
            int custId,
            byte roleId,
            int serverId,
            string ip,
            int subAccId)
        {
            CustId = custId;
            RoleId = roleId;
            ServerId = serverId;
            IP = ip;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public int ServerId { get; }

        public string IP { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_OnUser_ForceKickedout";

        public override bool IsValid()
            => CustId > 0 && RoleId > 0 && ServerId > 0;
    }
}
