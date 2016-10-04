namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Menus;

    public class MenuBarViewModel
    {
        private readonly Func<bool> menuBarVisibleFunc;
        private readonly Func<bool> accountInfoVisibleFunc;

        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems,
            bool isSmallDevice = false,
            Func<bool> menuBarVisibleFunc = null,
            Func<bool> accountInfoVisibleFunc = null)
        {
            MenuItems = menuItems.Where(menuItem => menuItem.Status.Visible);
            IsSmallDevice = isSmallDevice;
            this.menuBarVisibleFunc = menuBarVisibleFunc ?? FunctionRegistry.ShowMenuBar;
            this.accountInfoVisibleFunc = accountInfoVisibleFunc ?? FunctionRegistry.ShowAccountInfo;
        }

        public bool Visible
        {
            get { return menuBarVisibleFunc?.Invoke() ?? false;  }
        }

        public bool IsAccountInfoVisible
        {
            get { return accountInfoVisibleFunc?.Invoke() ?? false; }
        }

        public IEnumerable<IMenuItem> MenuItems { get; private set; }

        public bool IsSmallDevice { get; private set; }
    }
}