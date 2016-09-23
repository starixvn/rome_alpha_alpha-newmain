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

        public int CustId { get; private set; }

        public string UserName { get; private set; }

        public decimal Turnover { get; private set; }
    }
}