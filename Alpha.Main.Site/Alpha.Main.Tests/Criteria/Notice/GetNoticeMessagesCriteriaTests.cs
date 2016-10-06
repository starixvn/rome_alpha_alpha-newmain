namespace Alpha.Main.Tests.Criteria.Notice
{
    using System;
    using Main.Criteria.Notice;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GetNoticeMessagesCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_Return_Age_CustomerSelMessages()
        {
            // Arrange
            var criteria = new GetNoticeMessagesCriteria(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<int>(),
                Arg.Any<bool>());

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_CustomerSelMessages", actualSettingKey);
        }

        [TestCase(69, "69")]
        public void IsValid_TargetIdAndTargetId2IsValid_ReturnTrue(int targetId, string targetId2)
        {
            // Arrange
            var criteria = new GetNoticeMessagesCriteria(
                targetId,
                targetId2,
                Arg.Any<DateTime>(),
                Arg.Any<int>(),
                Arg.Any<bool>());

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(true, actual);
        }

        [TestCase(0, "")]
        [TestCase(-1, "")]
        public void IsValid_TargetIdAndTargetId2IsInvalid_ReturnFalse(int targetId, string targetId2)
        {
            // Arrange
            var criteria = new GetNoticeMessagesCriteria(
                targetId,
                targetId2,
                Arg.Any<DateTime>(),
                Arg.Any<int>(),
                Arg.Any<bool>());

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void LastPostTime_Always_ReturnValueSetWhenInit()
        {
            // Arrange
            var lastPostTime = DateTime.Today;
            var criteria = new GetNoticeMessagesCriteria(
                Arg.Any<int>(),
                Arg.Any<string>(),
                lastPostTime,
                Arg.Any<int>(),
                Arg.Any<bool>());

            // Act
            var actualLastPostTime = criteria.LastPostTime;

            // Assert
            Assert.AreEqual(lastPostTime, actualLastPostTime);
        }

        [Test]
        public void TotalMessage_Always_ReturnValueSetWhenInit()
        {
            // Arrange
            var totalMessage = 10;
            var criteria = new GetNoticeMessagesCriteria(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                totalMessage,
                Arg.Any<bool>());

            // Act
            var actualTotalMessage = criteria.TotalMessage;

            // Assert
            Assert.AreEqual(totalMessage, actualTotalMessage);
        }

        [Test]
        public void IsTest_Always_ReturnValueSetWhenInit()
        {
            // Arrange
            var criteria = new GetNoticeMessagesCriteria(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<int>(),
                false);

            // Act
            var actualIsTest = criteria.IsTest;

            // Assert
            Assert.AreEqual(false, actualIsTest);
        }
    }
}