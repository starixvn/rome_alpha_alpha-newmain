namespace Alpha.Main.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageConverter
    {
        // Length of LOB data to be replicated exceeds configured maximum 65536
        const long MAX_SIZE = 1024 * 20;
        const int STEP_LEVEL = 5;
        const int MIN_LEVEL = 5;
        const int MAX_LEVEL = 100;

        const int IMAGE_WIDTH = 240;
        const int IMAGE_HEIGHT = 240;

        public static byte[] ToLimitedImageSize(string base64String)
        {
            for (int level = MAX_LEVEL; level > MIN_LEVEL; level -= STEP_LEVEL)
            {
                var content = SaveJpg(Base64ToImage(base64String), level);

                if (content.Length <= MAX_SIZE)
                {
                    return content;
                }
            }

            return null;
        }

        private static ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i <= encoders.Length; i++)
            {
                if (encoders[i].MimeType == mime_type)
                {
                    return encoders[i];
                }
            }

            return null;
        }

        private static byte[] SaveJpg(Image image, int level)
        {
            EncoderParameters encoder_params = new EncoderParameters(1);
            encoder_params.Param[0] = new EncoderParameter(Encoder.Quality, level);
            ImageCodecInfo image_codec_info = GetEncoderInfo("image/jpeg");

            using (MemoryStream myStream = new MemoryStream())
            {
                image.Save(myStream, image_codec_info, encoder_params);
                return myStream.ToArray();
            }
        }

        private static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                var image = Image.FromStream(ms);
                var result = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT);
                using (var gr = Graphics.FromImage(result))
                {
                    gr.CompositingQuality = CompositingQuality.HighSpeed;
                    gr.SmoothingMode = SmoothingMode.HighSpeed;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.DrawImage(image, new Rectangle(0, 0, result.Width, result.Height));
                    result.Save(ms, ImageFormat.Jpeg);
                }

                return result;
            }
        }
    }
}
