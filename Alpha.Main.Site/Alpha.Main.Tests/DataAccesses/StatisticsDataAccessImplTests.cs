namespace Alpha.Main.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Database.Statistics;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria.Statistics;
    using Main.DataAccesses.Impl;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StatisticsDataAccessImplTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private StatisticsDataAccessImpl statisticsDataAccess;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            DbSettingProviderManager.StartTestingSession().Run();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((settingKey, commandTimeout) => objectDb);

            statisticsDataAccess = new StatisticsDataAccessImpl();
        }

        [Test]
        public void GetStatistics_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;
            var userLevel = new UserLevel();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsDataAccess.GetStatistics(criteria, userLevel));
        }

        [Test]
        public void GetStatistics_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);
            var expectedData = new Statistics();

            // Act
            var actualData = statisticsDataAccess.GetStatistics(new GetStatisticsCriteria(UserLevel.Agent, 1), UserLevel.Agent);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetStatistics_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var dataReader = StatisticsDatabase.StatisticsData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new Statistics(418754.50m, 1156415.20m, 32, 709, 0, 0, 0, 0, 0, 0, 0, 5);
            var criteria = new GetStatisticsCriteria(UserLevel.Agent, 1);

            // Act
            var actualData = statisticsDataAccess.GetStatistics(criteria, UserLevel.Agent);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetTopWinningMembers_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsDataAccess.GetTopWinningMembers(criteria));
        }

        [Test]
        public void GetTopWinningMembers_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);
            var expectedData = new List<TopWinningMember>();

            // Act
            var actualData = statisticsDataAccess.GetTopWinningMembers(
                new GetTopWinningMembersCriteria(UserLevel.Agent, 1, DateTime.Now, DateTime.Now));

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetTopWinningMembers_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("custid");
            dataTable.Columns.Add("username");
            dataTable.Columns.Add("winlost");
            dataTable.Rows.Add(1870092, "member16", 110.00m);

            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<TopWinningMember>
            {
                new TopWinningMember(1870092, "member16", 110.00m)
            };

            // Act
            var actualData = statisticsDataAccess.GetTopWinningMembers(
                new GetTopWinningMembersCriteria(UserLevel.Agent, 1, DateTime.Now, DateTime.Now));

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetTopTurnoverMembers_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsDataAccess.GetTopTurnoverMembers(criteria));
        }

        [Test]
        public void GetTopTurnoverMembers_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);
            var expectedData = new List<TopTurnoverMember>();

            // Act
            var actualData = statisticsDataAccess.GetTopTurnoverMembers(
                new GetTopTurnoverMembersCriteria(UserLevel.Agent, 1, DateTime.Now, DateTime.Now));

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetTopTurnoverMembers_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("custid");
            dataTable.Columns.Add("username");
            dataTable.Columns.Add("turnover");
            dataTable.Rows.Add(1870092, "member11", 49000.00m);
            dataTable.Rows.Add(1870093, "member01", 9800.00m);
            dataTable.Rows.Add(1870094, "member181", 2730.00m);

            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<TopTurnoverMember>
            {
                new TopTurnoverMember(1870092, "member11", 49000.00m),
                new TopTurnoverMember(1870093, "member01", 9800.00m),
                new TopTurnoverMember(1870094, "member181", 2730.00m)
            };

            // Act
            var actualData = statisticsDataAccess.GetTopTurnoverMembers(
                new GetTopTurnoverMembersCriteria(UserLevel.Agent, 1, DateTime.Now, DateTime.Now));

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}
