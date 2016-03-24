namespace Alpha.Main.Site.Models.Views
{
    using System.Collections.Generic;
    using Dtos;
    using Menus;
    using System.Linq;
    using System.Collections;

    public class NavigationViewModel
    {
        public bool IsMainMenuVisible { get; set; }

        public bool IsAccountInfoVisible { get; set; }

        public IEnumerable<IMenuItem> MenuItems { get; set; }

        public IEnumerable<IMenuItem> VisibleMenuItems
        {
            get
            {
                return MenuItems.Where(menuItem => menuItem.Status.Visible);
            }
        }
    }
}