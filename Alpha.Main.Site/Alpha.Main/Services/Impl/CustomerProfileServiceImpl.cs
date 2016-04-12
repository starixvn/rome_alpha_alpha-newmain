namespace Alpha.Main.Services.Impl
{
    using Criteria.CustomerProfile;
    using DataAccesses;
    using Models.CustomerProfiles;

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

            return _customerProfileDataAccess.GetProfilePicture(criteria);
        }
    }
}