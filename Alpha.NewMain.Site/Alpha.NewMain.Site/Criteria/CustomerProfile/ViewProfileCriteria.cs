namespace Alpha.NewMain.Site.Criteria.CustomerProfile
{
    using Fanex.Data.Repository;

    public class ViewProfileCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelProfilePicture";
        }
    }
}