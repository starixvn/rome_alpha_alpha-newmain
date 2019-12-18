namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Models;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Params;
    using Alpha.Main.ProfilePicture.Services;
    using Constants;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class ProfilePictureServicesTests
    {
        private CompareLogic compareLogic;
        private IDynamicRepository dynamicRepository;
        private ProfilePictureServices profilePictureServices;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            profilePictureServices = new ProfilePictureServicesImpl(dynamicRepository);
        }

        [Test]
        public void GetActorProfilePicture_InvalidCustId_ReturnNull()
        {
            // Arrange
            var expectedResult = new ActorProfilePictureModel();

            // Act
            var result = profilePictureServices.GetActorProfilePicture(CustId.InvalidId);

            // Assert
            Assert.True(compareLogic.Compare(result, expectedResult).AreEqual);
        }

        [Test]
        public void GetActorProfilePicture_ValidCustId_ReturnBalanceInfo()
        {
            // Arrange
            var expectedResult = FakeDataHelper.Create<ActorProfilePictureModel>();
            dynamicRepository.Get<ActorProfilePictureModel>(Arg.Any<ICriteria>()).Returns(expectedResult);

            // Act
            var actualResult = profilePictureServices.GetActorProfilePicture(CustId.ValidId);

            // Assert
            Assert.AreEqual(actualResult, expectedResult);
        }


        [Test]
        public void UpdateActorProfilePicture_InvalidCustId_ReturnNull()
        {
            // Arrange
            var expectedResult = new ActorProfilePictureModel();

            // Act
            var result = profilePictureServices.UpdateActorProfilePicture(
                new UpdateActorProfilePictureParam(CustId.InvalidId, 1, "192.168.0.1"));

            // Assert
            Assert.True(compareLogic.Compare(result, expectedResult).AreEqual);
        }
    }
}