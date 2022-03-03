namespace Alpha.Main.Criteria.Theme
{
    using Fanex.Data.Repository;

    public class LogChangeThemeCriteria : NonQueryCommand
    {
        public LogChangeThemeCriteria(
            int custId,
            int subAccId,
            bool isDarkMode,
            string ip,
            string location)
        {
            CustId = custId;
            SubAccId = subAccId;
            IsDarkMode = isDarkMode;
            IP = ip;
            Location = location;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public bool IsDarkMode { get; }

        public string IP { get; }

        public string Location { get; }

        public override string GetSettingKey()
            => "Age_SwitchDarkMode";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0;
    }
}
