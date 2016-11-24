namespace Alpha.Main.Models.Menus
{
    using System;

    public class MenuStatus
    {
        public MenuStatus(bool visible = true, bool disabled = false)
        {
            Visible = visible;
            Disabled = disabled;
        }

        public MenuStatus(Func<bool> getVisibleFunc = null, Func<bool> getDisableFunc = null)
            : this(
                getVisibleFunc != null ? getVisibleFunc.Invoke() : true,
                getDisableFunc != null ? getDisableFunc.Invoke() : false)
        {
        }

        public bool Visible { get; }

        public bool Disabled { get; }
    }
}