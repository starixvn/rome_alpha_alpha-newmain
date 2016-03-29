namespace Alpha.Main.Site.DataAccess.Impl
{
    using Alpha.Main.Site.Criteria.CustomerProfile;
    using Fanex.Common;
    using Fanex.Data;
    using Models.CustomerProfiles;

    public class CustomerProfileDataAccessImpl : CustomerProfileDataAccess
    {
        public ProfilePicture GetProfilePicture(GetProfilePictureCriteria criteria)
        {
            var profilePicture = new ProfilePicture();
            profilePicture.IsDefaultAvatar = true;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());
                if (reader.Read())
                {
                    profilePicture.IsDefaultAvatar = false;
                    profilePicture.BinaryAvatar = Converter.ToByteArray(reader["image"]);
                }
            }

            return profilePicture;
        }
    }
}