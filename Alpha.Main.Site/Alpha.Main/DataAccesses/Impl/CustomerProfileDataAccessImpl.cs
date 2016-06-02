namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using Alpha.Main.Criteria.CustomerProfile;
    using Fanex.Common;
    using Fanex.Data;
    using Models.CustomerProfiles;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Reviewed!")]
    public class CustomerProfileDataAccessImpl : CustomerProfileDataAccess
    {
        public ProfilePicture GetProfilePicture(GetProfilePictureCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

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