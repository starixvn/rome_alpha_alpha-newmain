namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Menus;
    using System.Web;
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
            this.menuBarVisibleFunc = menuBarVisibleFunc ?? FunctionRegistry.ShowMenuBar;
            this.accountInfoVisibleFunc = accountInfoVisibleFunc ?? FunctionRegistry.ShowAccountInfo;
        }

        public bool Visible
        {
            get { return this.menuBarVisibleFunc(); }
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
                bool isMoblie = false;
                string strUserAgent = HttpContext.Current.Request.UserAgent.ToLower();
                if (

                  strUserAgent.Contains("iphone")

                  || strUserAgent.Contains("ipod")

                 || strUserAgent.Contains("android")

                 || strUserAgent.Contains("iemobile")

                 || strUserAgent.Contains("version")

                 || strUserAgent.Contains("blackberry")

                 || strUserAgent.Contains("windows ce")

                 || strUserAgent.Contains("opera mini")

                 || strUserAgent.Contains("palm")

                 || strUserAgent.Contains("ipad"))
                {

                    return true;
                }

                return isMoblie;
            }
        }

        public string getUserAgent() {
                return HttpContext.Current.Request.UserAgent.ToLower();
        }
    }
}