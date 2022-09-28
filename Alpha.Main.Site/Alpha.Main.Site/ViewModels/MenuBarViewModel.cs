namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Main.Configurations;
    using Main.Models.Menus;

    public class MenuBarViewModel
    {
        private readonly Func<bool> accountInfoVisibleFunc;

        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems,
            bool isSmallDevice = false,
            Func<bool> accountInfoVisibleFunc = null,
            bool isDarkMode = false,
            string language = Main.Constants.Language.Default)
        {
            MenuItems = menuItems.Where(menuItem => menuItem.Status.Visible);
            IsSmallDevice = isSmallDevice;
            this.accountInfoVisibleFunc = accountInfoVisibleFunc ?? FunctionRegistry.ShowAccountInfo;
            IsDarkMode = isDarkMode;
            Language = Main.Constants.Language.MapToMainLanguage(language);
        }

        public bool IsAccountInfoVisible
        {
            get { return accountInfoVisibleFunc?.Invoke() ?? false; }
        }

        public IEnumerable<IMenuItem> MenuItems { get; }

        public bool IsSmallDevice { get; }

        public bool IsDarkMode { get; }

        public bool IsShowMenuBarBanner { get; } = AppSettings.IsShowMenuBarBanner;

        public string SiteGroup { get; } = AppSettings.SiteGroup;

        public string Language { get; }
    }
}