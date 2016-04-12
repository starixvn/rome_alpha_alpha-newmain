namespace Alpha.Main.DataAccesses
{
    using Alpha.Main.Criteria.CustomerProfile;
    using Models.CustomerProfiles;

    public interface CustomerProfileDataAccess
    {
        ProfilePicture GetProfilePicture(GetProfilePictureCriteria criteria);
    }
}