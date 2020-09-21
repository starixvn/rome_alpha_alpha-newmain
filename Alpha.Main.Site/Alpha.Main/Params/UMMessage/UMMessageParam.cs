namespace Alpha.Main.Params.UMMessage
{
    public class UMMessageParam
    {
        public UMMessageParam(
            int custId,
            string site,
            int currencyId,
            int superId)
        {
            CustId = custId;
            Site = site;
            CurrencyId = currencyId;
            SuperId = superId;
        }

        public int CustId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public int SuperId { get; }
    }
}
