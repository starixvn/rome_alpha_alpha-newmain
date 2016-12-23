namespace Alpha.Main.Tests.Helpers.Impl
{
    using System;
    using System.Threading;
    using Main.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class DateTimeWrapTests
    {
        private DateTimeWrap dateTimeWrap;

        [SetUp]
        public void SetUp()
        {
            dateTimeWrap = new DateTimeWrap();
        }

        [Test]
        public void GetDateTimeNow_Always_GetSystemTimeLessThan5Milliseconds()
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            // Act
            var actualDateTimeNow = dateTimeWrap.Now;

            // Assert
            Assert.LessOrEqual((actualDateTimeNow - dateTimeNow).TotalMilliseconds, 5);
        }

        [Test]
        public void SetDateTimeNow_Always_NotAssignTimeToWrapper()
        {
            // Arrange
            var dateTimeNow = DateTime.Now;

            // Act
            dateTimeWrap.Now = dateTimeNow;

            // Sleep 1 millisecond to change time
            Thread.Sleep(1);

            // Assert
            Assert.AreNotEqual(dateTimeNow, dateTimeWrap.Now);
        }
    }
}
