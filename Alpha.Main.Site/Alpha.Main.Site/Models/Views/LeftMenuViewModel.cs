namespace Alpha.Main.Site.Models.Views
{
    using System.Collections.Generic;
    using Dtos;

    public class LeftMenuViewModel
    {
        public bool IsShowMenu { get; set; }

        public bool ShowAccountInfo { get; set; }

        public List<MenuItem> MenuList { get; set; }
    }
}