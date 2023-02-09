using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.Feedback
{
    public class CheckExistsCriteria : NonQueryCommand
    {
        public CheckExistsCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public bool IsExists { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustFeedbackCheckExists";
        }

        public override bool IsValid() => CustId > 0;
    }
}
