namespace Alpha.Main.Tests.Models.Dashboard.Statistics
{
    using Helpers;
    using Main.Models.Statistics;
    using NUnit.Framework;

    [TestFixture]
    public class StatisticsTests
    {
        [Test]
        public void Outstanding_Always_SetValueFromCtor()
        {
            // Arrange
            decimal outstanding = 100;

            // Act
            var statistics = FakeDataHelper.Create<Statistics>("outStanding", outstanding);

            // Assert
            Assert.AreEqual(outstanding, statistics.Outstanding);
        }

        [Test]
        public void TotalOutstanding_Always_SetValueFromCtor()
        {
            // Arrange
            decimal totalOutstanding = 100;

            // Act
            var statistics = FakeDataHelper.Create<Statistics>("totalOutStanding", totalOutstanding);

            // Assert
            Assert.AreEqual(totalOutstanding, statistics.TotalOutstanding);
        }

        [Test]
        public void TotalMemberActive_Always_SetValueFromCtor()
        {
            // Arrange
            int totalMemberActive = 100;

            // Act
            var statistics = FakeDataHelper.Create<Statistics>("totalMemberActive", totalMemberActive);

            // Assert
            Assert.AreEqual(totalMemberActive, statistics.TotalMemberActive);
        }

        [Test]
        public void TotalMemberSuspended_Always_SetValueFromCtor()
        {
            // Arrange
            int totalMemberSuspended = 100;

            // Act
            var statistics = FakeDataHelper.Create<Statistics>("totalMemberSuspended", totalMemberSuspended);

            // Assert
            Assert.AreEqual(totalMemberSuspended, statistics.TotalMemberSuspended);
        }

        [Test]
        public void TotalMemberClosed_Always_SetValueFromCtor()
        {
            // Arrange
            int totalMemberClosed = 100;

            // Act
            var statistics = FakeDataHelper.Create<Statistics>("totalMemberClosed", totalMemberClosed);

            // Assert
            Assert.AreEqual(totalMemberClosed, statistics.TotalMemberClosed);
        }

        [Test]
        public void TotalAgentActive_Always_SetValueFromCtor()
        {
            // Arrange
            int totalAgentActive = 100;

            // Act
            var statistics = new Statistics(0, 0, 0, 0, 0, totalAgentActive, 0, 0, 0);

            // Assert
            Assert.AreEqual(totalAgentActive, statistics.TotalAgentActive);
        }

        [Test]
        public void TotalAgentSuspended_Always_SetValueFromCtor()
        {
            // Arrange
            int totalAgentSuspended = 100;

            // Act
            var statistics = new Statistics(0, 0, 0, 0, 0, 0, 0, totalAgentSuspended, 0);

            // Assert
            Assert.AreEqual(totalAgentSuspended, statistics.TotalAgentSuspended);
        }

        [Test]
        public void TotalAgentClosed_Always_SetValueFromCtor()
        {
            // Arrange
            int totalAgentClosed = 100;

            // Act
            var statistics = new Statistics(0, 0, 0, 0, 0, 0, totalAgentClosed, 0, 0);

            // Assert
            Assert.AreEqual(totalAgentClosed, statistics.TotalAgentClosed);
        }

        [Test]
        public void TotalMasterActive_Always_SetValueFromCtor()
        {
            // Arrange
            int totalMasterActive = 100;

            // Act
            var statistics = new Statistics(0, 0, 0, 0, 0, 0, 0, 0, totalMasterActive, 0, 0, 0);

            // Assert
            Assert.AreEqual(totalMasterActive, statistics.TotalMasterActive);
        }

        [Test]
        public void TotalMasterSuspended_Always_SetValueFromCtor()
        {
            // Arrange
            int totalMasterSuspended = 100;

            // Act
            var statistics = new Statistics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, totalMasterSuspended, 0);

            // Assert
            Assert.AreEqual(totalMasterSuspended, statistics.TotalMasterSuspended);
        }

        [Test]
        public void TotalMasterClosed_Always_SetValueFromCtor()
        {
            // Arrange
            int totalMasterClosed = 100;

            // Act
            var statistics = new Statistics(0, 0, 0, 0, 0, 0, 0, 0, 0, totalMasterClosed, 0, 0);

            // Assert
            Assert.AreEqual(totalMasterClosed, statistics.TotalMasterClosed);
        }
    }
}