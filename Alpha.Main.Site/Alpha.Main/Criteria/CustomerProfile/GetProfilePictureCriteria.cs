namespace Alpha.Main.Criteria.CustomerProfile
{
    using System;
    using Fanex.Data.Repository;

    public class GetProfilePictureCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelProfilePicture";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}