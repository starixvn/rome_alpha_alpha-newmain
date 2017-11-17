namespace Alpha.Main.Tests.DataAccesses
{
    using System.Data;
    using Fanex.Data;
    using Main.DataAccesses.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerProfileDataAccessImplTests
    {
        private IObjectDb objectDb;
        private CustomerProfileDataAccessImpl customerProfileDataAccess;

        [SetUp]
        public void SetUp()
        {
            DbSettingProviderManager.StartTestingSession().Run();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((settingKey, commandTimeout) => objectDb);

            customerProfileDataAccess = new CustomerProfileDataAccessImpl();
        }

        [Test]
        public void GetCustomerImageData_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(dataReader);
            var expectedData = new byte[] { };

            // Act
            var actualData = customerProfileDataAccess.GetCustomerImageData(1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
