namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using App_Start;
    using Models.Menus;

    public class MenuBarViewModel
    {
        private Func<bool> menuBarVisibleFunc;
        private Func<bool> accountInfoVisibleFunc;
        private IEnumerable<IMenuItem> menuItems;

        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems, 
            Func<bool> menuBarVisibleFunc = null, 
            Func<bool> accountInfoVisibleFunc = null)
        {
            this.menuItems = menuItems;
            this.menuBarVisibleFunc = menuBarVisibleFunc ?? FunctionConfig.ShowMenuBar;
            this.accountInfoVisibleFunc = accountInfoVisibleFunc ?? FunctionConfig.ShowAccountInfo;
        }

        public bool Visible
        {
            get { return this.menuBarVisibleFunc.Invoke(); }
        }

        public bool IsAccountInfoVisible
        {
            get { return this.accountInfoVisibleFunc.Invoke(); }
        }

        public IEnumerable<IMenuItem> MenuItems
        {
            get
            {
                return this.menuItems.Where(menuItem => menuItem.Status.Visible);
            }
        }
    }
}