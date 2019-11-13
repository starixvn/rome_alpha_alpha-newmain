namespace Alpha.Main.Services.Impl
{
    using System;
    using DataAccesses;
    using Fanex.Logging.Extensions.Web;
    using Models.CustomerProfiles;

    public class CustomerProfileServiceImpl : CustomerProfileService
    {
        private readonly CustomerProfileDataAccess customerProfileDataAccess;

        public CustomerProfileServiceImpl(CustomerProfileDataAccess customerProfileDataAccess)
        {
            this.customerProfileDataAccess = customerProfileDataAccess;
        }

        public ProfilePicture GetProfilePicture(int custId)
        {
            try
            {
                if (custId <= 0)
                {
                    throw new ArgumentException("customer Id is equal or less than zero", nameof(custId));
                }

                var customerImageData = customerProfileDataAccess.GetCustomerImageData(custId);

                if (customerImageData.Length == 0)
                {
                    return new ProfilePicture(custId, useDefaultAvatar: true);
                }

                return new ProfilePicture(custId, useDefaultAvatar: false, imageData: customerImageData);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return new ProfilePicture(custId, useDefaultAvatar: true);
            }
        }
    }
}