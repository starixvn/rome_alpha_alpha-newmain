namespace Alpha.Main.Tests.Services
{
    using Constants;
    using Criteria.CustomerProfile;
    using DataAccesses;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerProfileServiceImplTests
    {
        [TestCase(CustId.InvalidId, CustId.InvalidId)]
        [TestCase(CustId.ValidId, CustId.ValidId)]
        public void GetProfilePicture_AnyCustId_CallDataAccessGetProfilePicture(
            int custId,
            int expectedCustId)
        {
            // Arrange
            var customerProfileDataAccess = Substitute.For<CustomerProfileDataAccess>();

            var customerProfileService = new CustomerProfileServiceImpl(customerProfileDataAccess);

            // Action
            var profilePicture = customerProfileService.GetProfilePicture(custId);

            // Assert
            customerProfileDataAccess.Received().GetProfilePicture(Arg.Is<GetProfilePictureCriteria>(criteria => criteria.CustId == expectedCustId));
        }
    }
}
