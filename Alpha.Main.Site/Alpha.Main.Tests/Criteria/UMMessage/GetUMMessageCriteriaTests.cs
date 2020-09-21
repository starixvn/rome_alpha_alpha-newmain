namespace Alpha.Main.Tests.Criteria.UMMessage
{
    using Alpha.Main.Criteria.UMMessage;
    using NUnit.Framework;

    [TestFixture]
    public class GetUMMessageCriteriaTests
    {

        [Test]
        public void GetSettingKey_Always_ReturnValidSettingKey()
        {
            // Arrange
            var criteria = new GetUMMessageCriteria(1);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Acc_CheckUM", actualSettingKey);
        }

        [TestCase(10, true)]
        [TestCase(0, false)]
        [TestCase(-10, false)]
        public void IsValid_CustId_ReturnsCorrectExpected(int custId, bool expected)
        {
            // Arrange
            var criteria = new GetUMMessageCriteria(custId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
