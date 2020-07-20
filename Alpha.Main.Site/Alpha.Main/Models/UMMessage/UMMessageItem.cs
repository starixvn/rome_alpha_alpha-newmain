namespace Alpha.Main.Models.UMMessage
{
    using System;

    public class UMMessageItem
    {
        public UMMessageItem()
        {
        }

        public UMMessageItem(
            int productId,
            int sportType,
            int betType,
            bool isLive,
            byte settingType,
            DateTime beginTime,
            DateTime endTime,
            byte[] time_Stamp)
        {
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            IsLive = isLive;
            SettingType = settingType;
            BeginTime = beginTime;
            EndTime = endTime;
            Time_Stamp = time_Stamp;
        }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public bool IsLive { get; }

        public byte SettingType { get; }

        public DateTime BeginTime { get; }

        public DateTime EndTime { get; }

        public byte[] Time_Stamp { get; }
    }
}
