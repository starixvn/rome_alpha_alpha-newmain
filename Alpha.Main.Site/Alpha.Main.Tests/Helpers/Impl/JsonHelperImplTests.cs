using Alpha.Main.Helpers.Impl;
using Alpha.Main.Models.CustomerProfiles;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace Alpha.Main.Tests.Helpers.Impl
{
    [TestFixture]
    public class JsonHelperImplTests
    {
        private JsonHelperImpl jsonHelper;

        [SetUp]
        public void SetUp()
        {
            jsonHelper = new JsonHelperImpl();
        }

        [Test]
        public void Deserialize_Always_ReturnsCorrectObject()
        {
            // Arrange
            var json = "{\"CustId\":1,\"UseDefaultAvatar\":true,\"ImageData\":null}";
            var expectObject = new ProfilePicture(1, true);
            var compareLogic = new CompareLogic();

            // Act
            var actualObject = jsonHelper.Deserialize<ProfilePicture>(json);

            // Assert
            Assert.True(compareLogic.Compare(expectObject, actualObject).AreEqual);
        }

        [Test]
        public void Serialize_Always_ReturnsCorrectValue()
        {
            // Arrange
            var profilePicture = new ProfilePicture(1, true);
            var expectedResult = "{\"CustId\":1,\"UseDefaultAvatar\":true,\"ImageData\":null}";

            // Act
            var actualResult = jsonHelper.Serialize(profilePicture);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
