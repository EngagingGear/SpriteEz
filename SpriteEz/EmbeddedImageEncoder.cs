using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

// ReSharper disable once CheckNamespace
namespace SpriteEzNs
{
    public class EmbeddedImageEncoder
    {
        public string Encode(Image image)
        {
            using var m = new MemoryStream();
            image.Save(m, ImageFormat.Png);
            var imageBytes = m.ToArray();

            return Convert.ToBase64String(imageBytes);
        }
    }
}