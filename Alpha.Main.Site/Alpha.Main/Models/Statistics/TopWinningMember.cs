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

        public int CustId { get; set; }

        public string UserName { get; set; }

        public decimal WinLoss { get; set; }
    }
}