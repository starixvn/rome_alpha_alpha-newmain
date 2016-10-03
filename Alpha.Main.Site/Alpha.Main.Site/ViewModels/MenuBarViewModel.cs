namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Menus;

    public class MenuBarViewModel
    {
        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems,
            bool isSmallDevice = false,
            Func<bool> menuBarVisibleFunc = null,
            Func<bool> accountInfoVisibleFunc = null)
        {
            MenuItems = menuItems.Where(menuItem => menuItem.Status.Visible);
            IsSmallDevice = isSmallDevice;
            Visible = (menuBarVisibleFunc ?? FunctionRegistry.ShowMenuBar)();
            IsAccountInfoVisible = (accountInfoVisibleFunc ?? FunctionRegistry.ShowAccountInfo)();
        }

        public bool Visible { get; private set; }

        public bool IsAccountInfoVisible { get; private set; }

        public IEnumerable<IMenuItem> MenuItems { get; private set; }

        public bool IsSmallDevice { get; private set; }
    }
}