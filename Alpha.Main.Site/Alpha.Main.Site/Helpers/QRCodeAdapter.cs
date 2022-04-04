namespace Alpha.Main.Site.Helpers
{
    using Gma.QrCodeNet.Encoding;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using ZXing;
    using ZXing.Common;

    public class QRCodeAdapter
    {
        public static string Generate (string url, string logoPath)
        {
            var barcodeWriter = new BarcodeWriter();
            var encodingOptions = new EncodingOptions { Width = 100, Height = 100, Margin = 0, PureBarcode = false };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            
            Bitmap bitmap = barcodeWriter.Write(url);
            
            Bitmap overlay = new Bitmap(logoPath);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.DrawImage(overlay, new Point((bitmap.Width - overlay.Width) / 2, (bitmap.Height - overlay.Height) / 2));

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);

            return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
        }
    }
}