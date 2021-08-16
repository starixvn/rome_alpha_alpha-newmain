namespace Alpha.Main.Tests.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Menus;
    using NUnit.Framework;

    [TestFixture]
    public class MenuItemTests
    {
        private MenuItem menuItem;

        [Test]
        public void VisibileSubMenus_SubMenuItemsHasData_ReturnAllVisibledSubMenuItems()
        {
            // Arrange
            Predicate<string> authorizedUrlFunction = (a) =>
            {
                return true;
            };

            Predicate<string> unauthorizedUrlFunction = (a) =>
            {
                return false;
            };

            Func<MenuStatus> getMenuStatusFunc = () =>
            {
                return new MenuStatus(true, false);
            };

            // Act
            menuItem = new MenuItem
            {
                SubMenus = new List<IMenuItem>
                {
                    new MenuItem(getMenuStatusFunc, authorizedUrlFunction, null, () => false, () => false),
                    new MenuItem(getMenuStatusFunc, unauthorizedUrlFunction, null, () => false, () => false)
                }
            };

            // Assert
            Assert.AreEqual(1, menuItem.VisibileSubMenus.Count());
        }

        [Test]
        public void Status_UserIsNotAuthorizedOnMenu_ReturnStatusIsNotVisible()
        {
            // Arrange
            Predicate<string> authorizedUrlFunction = (a) =>
            {
                return false;
            };

            // Act
            var actualMenuItem = new MenuItem(null, authorizedUrlFunction, null, () => false, () => false);

            // Assert
            Assert.IsFalse(actualMenuItem.Status.Visible);
        }

        [Test]
        public void Status_UserIsAuthorizedOnMenu_ReturnMenuStatus()
        {
            // Arrange
            Predicate<string> authorizedUrlFunction = (a) =>
            {
                return true;
            };
            var expectedMenuStatus = new MenuStatus(true, false);
            Func<MenuStatus> getMenuStatusFunc = () =>
            {
                return expectedMenuStatus;
            };

            // Act
            var actualMenuItemStatus = new MenuItem(getMenuStatusFunc, authorizedUrlFunction, null, () => false, () => false).Status;

            // Assert
            Assert.AreEqual(expectedMenuStatus, actualMenuItemStatus);
        }

        [Test]
        public void DisplayName_GetDisplayNameFuncIsNotNull_ReturnValueOfFunc()
        {
            // Arrange
            Func<string> getDisplayName = () =>
            {
                return "DisplayName";
            };

            // Act
            var actualMenuItem = new MenuItem(null, null, getDisplayName, () => false, () => false);

            // Assert
            Assert.IsTrue(actualMenuItem.DisplayName.Equals("DisplayName"));
        }
    }
}
