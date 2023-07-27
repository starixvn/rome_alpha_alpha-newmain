namespace Alpha.Main.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria.SecurityCode;
    using Main.DataAccesses.Impl;
    using Main.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SecurityCodeDataAccessImplTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private SecurityCodeDataAccessImpl securityCodeDataAccess;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            DbSettingProviderManager.StartTestingSession().Run();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((settingKey, commandTimeout) => objectDb);

            securityCodeDataAccess = new SecurityCodeDataAccessImpl();
        }

        [Test]
        public void GetSecurityCode_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeDataAccess.GetSecurityCode(criteria));
        }

        [Test]
        public void GetSecurityCode_EmptyDataReturnFromDb_ReturnsEmpty()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            // Act
            var actualData = securityCodeDataAccess.GetSecurityCode(new GetMainAccountSecurityCodeCriteria());

            // Assert
            Assert.IsEmpty(actualData);
        }

        [Test]
        public void GetSecurityCode_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var expectedData = "d26aacf423f1b77ceda63fc7effd60a4f495ea76";
            var dataTable = new DataTable();
            dataTable.Columns.Add("");
            dataTable.Rows.Add(expectedData);

            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            // Act
            var actualData = securityCodeDataAccess.GetSecurityCode(new GetMainAccountSecurityCodeCriteria());

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        public void UpdateSecurityCode_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeDataAccess.UpdateSecurityCode(criteria));
        }

        [Test]
        public void UpdateSecurityCode_UpdateSuccessfully_ReturnsSuccessedResult()
        {
            // Arrange
            var expectedResult = new Result
            {
                Code = 0,
                Message = "Successfully updated."
            };

            var outputParams = new Hashtable()
            {
                ["errcode"] = 0,
                ["errmsg"] = string.Empty
            };
            objectDb.GetOutputParams().Returns(outputParams);

            // Act
            var actualResult = securityCodeDataAccess.UpdateSecurityCode(new UpdateMainAccountSecurityCodeCriteria());

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }

        [Test]
        public void UpdateSecurityCode_UpdateSuccessfully_ReturnsFailedResult()
        {
            // Arrange
            var expectedResult = new Result
            {
                Code = 1,
                Message = "ErrorMessage"
            };

            var outputParams = new Hashtable()
            {
                ["errcode"] = 1,
                ["errmsg"] = "ErrorMessage"
            };
            objectDb.GetOutputParams().Returns(outputParams);

            // Act
            var actualResult = securityCodeDataAccess.UpdateSecurityCode(new UpdateSubAccountSecurityCodeCriteria());

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }
    }
}