namespace Alpha.Main.Tests.Criteria.UMMessage
{
    using Alpha.Main.Criteria.UMMessage;
    using Alpha.Main.Params.UMMessage;
    using NUnit.Framework;

    [TestFixture]
    public class GetUMMessageCriteriaTests
    {
        [Test]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange
            var param = new UMMessageParam(1,  "en-US");

            // Act
            var criteria = new GetUMMessageCriteria(param);

            // Assert
            Assert.IsTrue(1 == criteria.CustId
                && null == criteria.ProductId
                && null == criteria.SportType
                && null == criteria.SettingType
                && null == criteria.FunctionType
                && 0 == criteria.ErrCode
                && null == criteria.ErrMsg);
        }

        [Test]
        public void GetSettingKey_Always_ReturnValidSettingKey()
        {
            // Arrange
            var param = new UMMessageParam(1, "en-US");
            var criteria = new GetUMMessageCriteria(param);

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
            var param = new UMMessageParam(custId, "en-US");
            var criteria = new GetUMMessageCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
