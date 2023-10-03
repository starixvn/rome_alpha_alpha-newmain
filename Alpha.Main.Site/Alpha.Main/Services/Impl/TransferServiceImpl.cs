namespace Alpha.Main.Services.Impl
{
    using System;
    using Alpha.Main.Criteria.Transfer;
    using Alpha.Main.DataAccesses;
    using Alpha.Main.DataAccesses.Impl;
    using System.Linq;

    public class TransferServiceImpl : TransferService
    {
        private readonly TransferDataAccess transferDataAccess;

        public TransferServiceImpl(TransferDataAccess transferDataAccess = null)
            => this.transferDataAccess = transferDataAccess ?? new TransferDataAccessImpl();

        public bool IsAllDayBalanceTransfer(int custId)
            => transferDataAccess.IsAllDayBalanceTransfer(new GetIsAllDayBalTransferCriteria(custId));

        public bool IsEnableAutoTransfer(string currency, int custId, string offerCurrencies)
        {
            if (IsAllDayBalanceTransfer(custId))
            {
                return false;
            }

            var s = offerCurrencies;
            if (string.IsNullOrWhiteSpace(offerCurrencies))
            {
                return false;
            }

            if (offerCurrencies == "*")
            {
                return true;
            }

            return offerCurrencies.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(c => string.Equals(currency, c.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
