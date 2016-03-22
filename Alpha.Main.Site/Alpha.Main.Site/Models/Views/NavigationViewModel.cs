namespace Alpha.Main.Site.Models.Views
{
    using System.Collections.Generic;
    using Dtos;
    using Menus;

    public class NavigationViewModel
    {
        public bool IsShowMenu { get; set; }

        public bool ShowAccountInfo { get; set; }

        public IEnumerable<IMenuItem> MenuList { get; set; }
    }
}