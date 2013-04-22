using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;

namespace voidsoft.Zinc
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageUtilities
    {

        public static string GenerateThumbnail(string imagePath, int thumbWidth, int thumbHeight)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite);

                Bitmap image = (Bitmap)Image.FromStream(fs);

                Bitmap resized = (Bitmap)image.GetThumbnailImage(thumbWidth, thumbHeight, () => true, IntPtr.Zero);

                string name = Guid.NewGuid().ToString();

                string newImage = Path.GetDirectoryName(imagePath) + Path.DirectorySeparatorChar + name + ".png";

                resized.Save(newImage, ImageFormat.Png);

                return newImage;
            }
            finally
            {

                if (fs != null)
                {
                    fs.Close();
                }
            }


        }

        /// <summary>
        /// Resizes the uploaded image.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ResizeUploadedImage(string imagePath, int width, int height)
        {

            FileStream fs = null;

            try
            {
                fs = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite);
                Bitmap image = (Bitmap)Image.FromStream(fs);

                if (image.Height == width && image.Height == height)
                {
                    return;
                }

                image.SetResolution(width, height);
                image.Save(fs, ImageFormat.Png);
            }
            finally
            {

                if (fs != null)
                {
                    fs.Close();
                }
            }


        }
    }
}