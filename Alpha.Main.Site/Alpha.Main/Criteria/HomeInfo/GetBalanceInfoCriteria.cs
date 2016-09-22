namespace Alpha.Main.Criteria.HomeInfo
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetBalanceInfoCriteria : CriteriaBase
    {
        private string settingKey;

        public GetBalanceInfoCriteria(string settingKey)
        {
            this.settingKey = settingKey;
        }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            return this.settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
