namespace Alpha.Main.Tests.Services
{
    using Alpha.Web.Models;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using Params;

    [TestFixture]
    public class KickoutCustomerServiceImplTests
    {
        private CompareLogic compareLogic;
        private IDynamicRepository dynamicRepository;
        private KickoutCustomerService kickoutCustomerService;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();

            dynamicRepository = Substitute.For<IDynamicRepository>();
            kickoutCustomerService = new KickoutCustomerServiceImpl(dynamicRepository);
        }

        [Test]
        public void Kickout_ValidateCriteriaSuccessful_CallsExecute()
        {
            // Arrange
            var param = new KickoutCustomerParam(1, UserLevel.Master, 90, "::1", 0);

            // Act
            kickoutCustomerService.Kickout(param);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<KickoutCustomerCriteria>(
                c => c.CustId == 1
                && c.RoleId == 3
                && c.ServerId == 90
                && c.IP == "::1"
                && c.SubAccId == 0));
        }
    }
}
