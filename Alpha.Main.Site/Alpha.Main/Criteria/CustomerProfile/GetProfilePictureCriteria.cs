namespace Alpha.Main.Criteria.CustomerProfile
{
    using Fanex.Data.Repository;

    public class GetProfilePictureCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelProfilePicture";
        }
    }
}