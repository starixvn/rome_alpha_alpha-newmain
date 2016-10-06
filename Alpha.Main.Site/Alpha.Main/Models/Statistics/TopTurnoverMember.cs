namespace Alpha.Main.Models.Statistics
{
    public class TopTurnoverMember
    {
        public TopTurnoverMember(int custId, string userName, decimal turnOver)
        {
            CustId = custId;
            UserName = userName;
            Turnover = turnOver;
        }

        public int CustId { get; }

        public string UserName { get; }

        public decimal Turnover { get; }
    }
}