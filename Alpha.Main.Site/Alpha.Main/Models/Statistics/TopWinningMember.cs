namespace Alpha.Main.Models.Statistics
{
    public class TopWinningMember
    {
        public TopWinningMember(int custId, string userName, decimal winLoss)
        {
            CustId = custId;
            UserName = userName;
            WinLoss = winLoss;
        }

        public int CustId { get; }

        public string UserName { get; }

        public decimal WinLoss { get; }
    }
}