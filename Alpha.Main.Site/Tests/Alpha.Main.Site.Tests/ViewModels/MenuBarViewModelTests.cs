using Alpha.Main.Models.Menus;
using Alpha.Main.Site.ViewModels;
using NUnit.Framework;
using System.Linq;

namespace Alpha.Main.Site.Tests.ViewModels
{
    [TestFixture]
    public class MenuBarViewModelTests
    {
        [Test]
        public void IsAccountInfoVisible_AccountInfoVisibleFuncIsFalse_ReturnsFalse()
        {
            // Arrange
            var viewModel = new MenuBarViewModel(Enumerable.Empty<IMenuItem>(), true, () => false);

            // Act
            var value = viewModel.IsAccountInfoVisible;

            // Assert
            Assert.False(value);
        }

        [Test]
        public void IsAccountInfoVisible_AccountInfoVisibleFuncIsTrue_ReturnsTrue()
        {
            // Arrange
            var viewModel = new MenuBarViewModel(Enumerable.Empty<IMenuItem>(), true, () => true);

            // Act
            var value = viewModel.IsAccountInfoVisible;

            // Assert
            Assert.True(value);
        }
    }
}
