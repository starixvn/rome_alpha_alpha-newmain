using System;
using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.Customer
{
    public class UpdateSyncedNovaCasinoCommand : NonQueryCommand
    {
        public UpdateSyncedNovaCasinoCommand(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerUpdSynced";

        public override bool IsValid() => CustId > 0;
    }
}