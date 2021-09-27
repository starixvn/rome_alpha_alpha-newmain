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

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public byte SettingType { get; set; }

        public byte FunctionType { get; set; }

        public DateTime BeginTime { get; set; }

        public string BeginTimeString { get; set; }

        public DateTime EndTime { get; set; }

        public string EndTimeString { get; set; }

        public byte[] Time_Stamp { get; set; }
    }
}
