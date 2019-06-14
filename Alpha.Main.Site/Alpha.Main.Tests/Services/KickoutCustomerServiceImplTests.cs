namespace Alpha.Main.Tests.Services
{
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria;
    using Main.Models.Enumerations;
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
            var criteria = new KickoutCustomerCriteria(param);

            // Act
            kickoutCustomerService.Kickout(param);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<KickoutCustomerCriteria>(
                c => compareLogic.Compare(c, criteria).AreEqual));
        }
    }
}
