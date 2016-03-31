namespace Alpha.Main.Site.Helpers
{
    using System;
    using System.Diagnostics;

    public static class Argument
    {
        [DebuggerStepThrough]
        public static void Requires(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        [DebuggerStepThrough]
        public static void Requires(bool condition)
        {
            if (!condition)
            {
                throw new InvalidOperationException();
            }
        }
    }
}