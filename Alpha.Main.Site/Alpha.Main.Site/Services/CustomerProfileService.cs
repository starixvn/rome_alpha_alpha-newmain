namespace Alpha.Main.Site.Services
{
    using Models.CustomerProfiles;

    public interface CustomerProfileService
    {
        ProfilePicture GetProfilePicture(int custId);
    }
}