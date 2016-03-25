namespace Alpha.Main.Site.DataAccess
{
    using Alpha.Main.Site.Criteria.CustomerProfile;
    using Alpha.Main.Site.Models.Dtos;
    using Models.CustomerProfiles;

    public interface CustomerProfileDataAccess
    {
        ProfilePicture GetProfilePicture(GetProfilePictureCriteria criteria);
    }
}