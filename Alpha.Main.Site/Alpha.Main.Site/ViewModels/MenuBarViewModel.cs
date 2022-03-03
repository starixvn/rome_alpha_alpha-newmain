namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Menus;

    public class MenuBarViewModel
    {
        private readonly Func<bool> accountInfoVisibleFunc;

        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems,
            bool isSmallDevice = false,
            Func<bool> accountInfoVisibleFunc = null,
            bool isDarkMode = false)
        {
            MenuItems = menuItems.Where(menuItem => menuItem.Status.Visible);
            IsSmallDevice = isSmallDevice;
            this.accountInfoVisibleFunc = accountInfoVisibleFunc ?? FunctionRegistry.ShowAccountInfo;
            IsDarkMode = isDarkMode;
        }

        public bool IsAccountInfoVisible
        {
            get { return accountInfoVisibleFunc?.Invoke() ?? false; }
        }

        public IEnumerable<IMenuItem> MenuItems { get; }

        public bool IsSmallDevice { get; }

        public bool IsDarkMode { get; }
    }
}