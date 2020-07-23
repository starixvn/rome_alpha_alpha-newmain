namespace Alpha.Main.Params.UMMessage
{
    public class UMMessageParam
    {
        public UMMessageParam(
            int custId,
            string language) : this(custId, null, null, null, null, language)
        {
        }

        private UMMessageParam(
            int custId,
            int? productId,
            int? sportType,
            byte? settingType,
            byte? functionType,
            string language)
        {
            CustId = custId;
            ProductId = productId;
            SportType = sportType;
            SettingType = settingType;
            FunctionType = functionType;
            Language = language;
        }

        public int CustId { get; }

        public int? ProductId { get; }

        public int? SportType { get; }

        public byte? SettingType { get; }

        public byte? FunctionType { get; }

        public string Language { get; }
    }
}
