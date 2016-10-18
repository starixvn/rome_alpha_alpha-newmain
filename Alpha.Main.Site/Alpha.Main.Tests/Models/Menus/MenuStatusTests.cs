namespace Alpha.Main.Tests.Models.Menus
{
    using Main.Models.Menus;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class MenuStatusTests
    {
        [Test]
        public void MenuStatus_GetVisibleFunctionIsNull_ReturnVisibleIsTrue()
        {
            //// Act
            var actualMenuStatus = new MenuStatus(null, null);

            //// Assert
            Assert.IsTrue(actualMenuStatus.Visible);
        }

        [Test]
        public void MenuStatus_GetDisableFunctionIsNull_ReturnDisableIsFalse()
        {
            //// Act
            var actualMenuStatus = new MenuStatus(null, null);

            //// Assert
            Assert.IsFalse(actualMenuStatus.Disabled);
        }

        [Test]
        public void MenuStatus_CreateConstructor_InvokeGetVisibleFunction()
        {
            //// Arrange
            var getVisibleFunc = Substitute.For<Func<bool>>();

            //// Act
            var actualMenuStatus = new MenuStatus(getVisibleFunc, null);

            //// Assert
            getVisibleFunc.Received().Invoke();
        }

        [Test]
        public void MenuStatus_CreateConstructor_InvokeGetDisableFunction()
        {
            //// Arrange
            var getDisableFunc = Substitute.For<Func<bool>>();

            //// Act
            var actualMenuStatus = new MenuStatus(null, getDisableFunc);

            //// Assert
            getDisableFunc.Received().Invoke();
        }
    }
}