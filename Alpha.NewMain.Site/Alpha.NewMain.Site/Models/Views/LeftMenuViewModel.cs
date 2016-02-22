namespace Alpha.NewMain.Site.Models.Views
{
    using System.Collections.Generic;
    using Dtos;

    public class LeftMenuViewModel
    {
        public bool IsShowMenu { get; set; }

        public bool ShowAccountInfo { get; set; }

        public List<MenuModel> MenuList { get; set; }
    }
}