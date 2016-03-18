namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class MenuStatus
    {                
        public MenuStatus(bool isVisible = true, bool isDisabled = false)
        {
            this.IsVisible = isVisible;
            this.IsDisabled = isDisabled;
        }

        public bool IsVisible { get; set; }

        public bool IsDisabled { get; set; }

    }
}