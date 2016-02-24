namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.IO;

    public class FileUploadService : IFileUploadService
    {
        private static char _dirSeparator = Path.DirectorySeparatorChar;

        private static string _fileDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public byte[] GetDefaultAvatar()
        {
            byte[] content;
            string pathDefaultImage = _fileDirectory + "Content\\Images";
            if (!Directory.Exists(pathDefaultImage))
            {
                Directory.CreateDirectory(pathDefaultImage);
            }

            string imagePath = string.Format(@"{0}{1}{2}", pathDefaultImage, _dirSeparator, "icon_user_default.png");
            FileStream stream = new FileStream(Path.GetFullPath(imagePath), FileMode.OpenOrCreate);
            content = StreamToByteArray(stream);
            string a = BitConverter.ToString(content);
            stream.Close();
            return content;
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }
    }
}