namespace Alpha.Main.Tests.Services
{
    using System;
    using DataAccesses;
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

        [TestCase(0)]
        [TestCase(-100)]
        public void GetProfilePicture_CustIdLessThanOrEqualToZero_ThrowArgumentException(int custId)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => customerProfileService.GetProfilePicture(custId));
        }

        [Test]
        public void GetProfilePicture_CustomerImageDataIsEmpty_DefaultAvatarIsTrue()
        {
            // Arrange
            var custId = 100;            
            byte[] emptyImageData = new byte[] { };
            customerProfileDataAccess
                .GetCustomerImageData(custId)
                .ReturnsForAnyArgs(emptyImageData);

            // Act
            var customerProfile = customerProfileService.GetProfilePicture(custId);

            // Assert
            Assert.AreEqual(true, customerProfile.UseDefaultAvatar);
        }

        [Test]
        public void GetProfilePicture_CustomerImageDataIsNotEmpty_DefaultAvatarIsFalse()
        {
            // Arrange
            var custId = 100;
            byte[] imageData = new byte[] { 68 };
            customerProfileDataAccess
                .GetCustomerImageData(custId)
                .ReturnsForAnyArgs(imageData);

            // Act
            var customerProfile = customerProfileService.GetProfilePicture(custId);

            // Assert
            Assert.AreEqual(false, customerProfile.UseDefaultAvatar);
        }

        [Test]
        public void GetProfilePicture_CustomerImageDataIsNotEmpty_DataWillBetSetToBinaryAvatar()
        {
            // Arrange
            var custId = 100;
            byte[] imageData = new byte[] { 68 };
            customerProfileDataAccess
                .GetCustomerImageData(custId)
                .ReturnsForAnyArgs(imageData);

            // Act
            var customerProfile = customerProfileService.GetProfilePicture(custId);

            // Assert
            Assert.AreEqual(imageData, customerProfile.ImageData);
        }
    }
}