namespace Alpha.Main.Criteria.UMMessage
{
    using Alpha.Main.Params.UMMessage;
    using Fanex.Data.Repository;

    public class GetUMMessageCriteria : CriteriaBase
    {
        public GetUMMessageCriteria(UMMessageParam param)
        {
            CustId = param.CustId;
            ProductId = param.ProductId;
            SportType = param.SportType;
            SettingType = param.SettingType;
            FunctionType = param.FunctionType;
        }

        public int CustId { get; }

        public int? ProductId { get; }

        public int? SportType { get; }

        public byte? SettingType { get; }

        public byte? FunctionType { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_CheckUM";

        public override bool IsValid()
            => CustId > 0;
    }
}
