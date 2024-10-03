using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.WhiteLabel
{
    public class CheckExistsWhiteLabelCommand : NonQueryCommand
    {
        public CheckExistsWhiteLabelCommand(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public bool IsExists { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_WhitelLabelCheckExists";
        }
    }
}