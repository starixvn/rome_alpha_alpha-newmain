namespace Alpha.Main.Models.Home
{
    using System;

    public class BalanceUpDayInfo
    {
        public BalanceUpDayInfo(DateTime balanceUpDay, DateTime moveBetsDay)
        {
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        public DateTime BalanceUpDay { get; private set; }

        public DateTime MoveBetsDay { get; private set; }
    }
}
