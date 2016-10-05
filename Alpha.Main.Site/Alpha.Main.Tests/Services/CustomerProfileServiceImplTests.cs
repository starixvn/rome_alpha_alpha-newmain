namespace Alpha.Main.Tests.Services
{
    using Constants;
    using DataAccesses;
    using Main.Criteria.CustomerProfile;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerProfileServiceImplTests
    {
        private CustomerProfileDataAccess customerProfileDataAccess;
        private CustomerProfileServiceImpl customerProfileService;

        [SetUp]
        public void SetUp()
        {
            customerProfileDataAccess = Substitute.For<CustomerProfileDataAccess>();
            customerProfileService = new CustomerProfileServiceImpl(customerProfileDataAccess);
        }

        [TestCase(CustId.InvalidId, CustId.InvalidId)]
        [TestCase(CustId.ValidId, CustId.ValidId)]
        public void GetProfilePicture_AnyCustId_CallDataAccessGetProfilePicture(
            int custId,
            int expectedCustId)
        {
            // Arrange

            // Act
            var profilePicture = customerProfileService.GetProfilePicture(custId);

            // Assert
            customerProfileDataAccess.Received().GetProfilePicture(Arg.Is<GetProfilePictureCriteria>(criteria => criteria.CustId == expectedCustId));
        }
    }
}