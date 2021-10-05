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
            byte settingType,
            byte functionType,
            DateTime beginTime,
            DateTime endTime,
            byte[] time_Stamp)
        {
            ProductId = productId;
            SportType = sportType;
            SettingType = settingType;
            FunctionType = functionType;
            BeginTime = beginTime;
            EndTime = endTime;
            Time_Stamp = time_Stamp;
        }

        public int ProductId { get; }

        public int SportType { get; }

        public byte SettingType { get; }

        public byte FunctionType { get; }

        public DateTime BeginTime { get; }

        public DateTime EndTime { get; }

        public byte[] Time_Stamp { get; }
    }
}
