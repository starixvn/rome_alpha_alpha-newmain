namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Criteria.CustomerProfile;
    using DataAccess;
    using Extensions;
    using Models.CustomerProfiles;
    using Models.Dtos;

    public class CustomerProfileServiceImpl : CustomerProfileService
    {
        private readonly CustomerProfileDataAccess _customerProfileDataAccess;

        public CustomerProfileServiceImpl(CustomerProfileDataAccess customerProfileDataAccess)
        {
            _customerProfileDataAccess = customerProfileDataAccess;
        }

        public ProfilePicture GetProfilePicture(int custId)
        {
            var criteria = new GetProfilePictureCriteria
            {
                CustId = custId
            };

            return _customerProfileDataAccess.GetProfilePicture(criteria); ;
        }
    }
}