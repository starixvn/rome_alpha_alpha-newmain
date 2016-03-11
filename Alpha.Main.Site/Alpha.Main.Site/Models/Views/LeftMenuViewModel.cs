namespace Alpha.Main.Site.Models.Views
{
    using System.Collections.Generic;
    using Dtos;

    public class SideBarViewModel
    {
        public bool IsShowMenu { get; set; }

        public bool ShowAccountInfo { get; set; }

        public List<MenuItem> MenuList { get; set; }
    }
}