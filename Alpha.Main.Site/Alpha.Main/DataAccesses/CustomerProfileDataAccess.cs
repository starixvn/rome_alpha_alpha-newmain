namespace Alpha.Main.DataAccesses
{
    using Alpha.Main.Criteria.CustomerProfile;
    using Models.CustomerProfiles;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", MessageId = "I", Justification = "Reviewed!")]
    public interface CustomerProfileDataAccess
    {
        ProfilePicture GetProfilePicture(GetProfilePictureCriteria criteria);
    }
}