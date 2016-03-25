namespace Alpha.Main.Site.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models.CustomerProfiles;
    using Models.Dtos;
    using Models.Requests.CustomerProfile;

    public interface CustomerProfileService
    {
        ProfilePicture GetProfilePicture(int custId);
    }
}
