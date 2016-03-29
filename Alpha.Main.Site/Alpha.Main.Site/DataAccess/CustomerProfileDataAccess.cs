namespace Alpha.Main.Site.DataAccess
{
    using Alpha.Main.Site.Criteria.CustomerProfile;
    using Models.CustomerProfiles;

    public interface CustomerProfileDataAccess
    {
        ProfilePicture GetProfilePicture(GetProfilePictureCriteria criteria);
    }
}