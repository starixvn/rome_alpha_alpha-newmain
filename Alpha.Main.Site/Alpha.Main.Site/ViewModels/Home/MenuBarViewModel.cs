namespace Alpha.Main.Site.ViewModels
{
    using Main.Models.Menus;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MenuBarViewModel
    {
        private Func<bool> accountInfoVisibleFunc;
        private IEnumerable<IMenuItem> menuItems;
        private bool isSmallDevice;

        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems,
            bool isSmallDevice = false,
            Func<bool> accountInfoVisibleFunc = null)
        {
            this.menuItems = menuItems;
            this.isSmallDevice = isSmallDevice;

            this.accountInfoVisibleFunc = accountInfoVisibleFunc ?? FunctionRegistry.ShowAccountInfo;
        }

        public bool IsAccountInfoVisible
        {
            get { return this.accountInfoVisibleFunc(); }
        }

        public IEnumerable<IMenuItem> MenuItems
        {
            get
            {
                return this.menuItems.Where(menuItem => menuItem.Status.Visible);
            }
        }

        public bool IsSmallDevice
        {
            get
            {
                return this.isSmallDevice;
            }
        }
    }
}