using Alpha.Main.DataAccesses.Impl;
using Alpha.Main.Models.Enumerations;
using Fanex.Data.Repository;
using NUnit.Framework;
using System;

namespace Alpha.Main.Tests.DataAccesses
{
    [TestFixture]
    public class StatisticsDataAccessImplTests
    {
        private StatisticsDataAccessImpl statisticsDataAccess;

        [SetUp]
        public void SetUp()
        {
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
        public void GetTopWinningMembers_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsDataAccess.GetTopWinningMembers(criteria));
        }

        [Test]
        public void GetTopTurnoverMembers_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statisticsDataAccess.GetTopTurnoverMembers(criteria));
        }
    }
}
