namespace Alpha.Main.Tests.Helps
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class FileHelper
    {
        public static string GetFilePath(string relativeFilePath)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path) + relativeFilePath;
        }
    }
}
