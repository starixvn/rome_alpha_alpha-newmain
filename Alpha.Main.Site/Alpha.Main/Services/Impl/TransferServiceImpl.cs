namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Linq;
    using Alpha.Main.Criteria.Transfer;
    using Alpha.Main.Models;
    using Fanex.Data.Repository;

    public class TransferServiceImpl : TransferService
    {
        private readonly IDynamicRepository dynamicRepository;

        public TransferServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public bool IsAllDayBalanceTransfer(int custId)
        {
            var transferModel = dynamicRepository.Get<TransferModel>(new GetIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

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
