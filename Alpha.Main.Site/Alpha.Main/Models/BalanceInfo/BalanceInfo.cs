namespace Alpha.Main.Models.BalanceInfo
{
    public class BalanceInfo
    {
        public BalanceInfo(
            string userName,
            decimal cashBalance,
            decimal yesterdayCashBalance,
            decimal totalBalance,
            decimal yesterdayTotalBalance,
            decimal todayWinLoss,
            decimal yesterdayWinLoss,
            decimal givenCredit,
            decimal memberCredit)
        {
            UserName = userName;
            CashBalance = cashBalance;
            YesterdayCashBalance = yesterdayCashBalance;
            TotalBalance = totalBalance;
            YesterdayTotalBalance = yesterdayTotalBalance;
            TodayWinLoss = todayWinLoss;
            YesterdayWinLoss = yesterdayWinLoss;
            GivenCredit = givenCredit;
            MemberCredit = memberCredit;
        }

        public string UserName { get; private set; }

        public decimal CashBalance { get; private set; }

        public decimal YesterdayCashBalance { get; private set; }

        public decimal TotalBalance { get; private set; }

        public decimal YesterdayTotalBalance { get; private set; }

        public decimal TodayWinLoss { get; private set; }

        public decimal YesterdayWinLoss { get; private set; }

        public decimal GivenCredit { get; private set; }

        public decimal MemberCredit { get; private set; }
    }
}