using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace MyCharter.Util
{
    public static class ImageMethods
    {
        /// <summary>
        /// Check to see if the Bitmap contains any pixels other than the backgroundColor.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        /// <param name="cloneRectangle"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="OutputFilePathOnCollision">On collision (if the space is not empty), then output the rectangle to this filename.</param>
        /// <returns></returns>
        public static bool IsSpaceEmpty(Graphics g, Bitmap bmp, Rectangle cloneRectangle, Color backgroundColor, string OutputFilePathOnCollision)
        {
            bool rValue = true;
            OutputFilePathOnCollision = OutputFilePathOnCollision.Replace("/", "");

            if (cloneRectangle.X > bmp.Width || cloneRectangle.X + cloneRectangle.Width > bmp.Width ||
                cloneRectangle.Y > bmp.Height || cloneRectangle.Y + cloneRectangle.Height > bmp.Height)
            {
                Console.WriteLine("Dropping out; mismatch between bmp size and cloneRectangle");
                return true;
            }

            Bitmap cloneBitmap = bmp.Clone(cloneRectangle, bmp.PixelFormat);

            for (int x = 0; x < cloneBitmap.Width; x++)
            {
                for (int y = 0; y < cloneBitmap.Height; y++)
                {
                    if (cloneBitmap.GetPixel(x,y).ToArgb() != backgroundColor.ToArgb())
                    {
                        rValue = false;
                        if (OutputFilePathOnCollision != null)
                            cloneBitmap.Save(OutputFilePathOnCollision);
                        break;
                    }
                }
            }

            return rValue;
        }

        /// <summary>
        /// Check to see if the Bitmap contains any pixels other than the backgroundColor.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        /// <param name="cloneRectangle"></param>
        /// <param name="ignoreColors">Colors to ignore (i.e. treat as if it's not there)</param>
        /// <param name="outputFilePathOnCollision">On collision (if the space is not empty), then output the rectangle to this filename.</param>
        /// <returns></returns>
        public static bool IsSpaceEmpty(Graphics g, Bitmap bmp, Rectangle cloneRectangle, List<int> ignoreColors,
            string outputFilePathOnCollision)
        {
            bool rValue = true;
            if (outputFilePathOnCollision != null)
                outputFilePathOnCollision = outputFilePathOnCollision.Replace("/", "");

            if (cloneRectangle.X > bmp.Width || cloneRectangle.X + cloneRectangle.Width > bmp.Width ||
                cloneRectangle.Y > bmp.Height || cloneRectangle.Y + cloneRectangle.Height > bmp.Height)
            {
                Console.WriteLine("Dropping out; mismatch between bmp size and cloneRectangle");
                return true;
            }

            Bitmap cloneBitmap = bmp.Clone(cloneRectangle, bmp.PixelFormat);

            for (int x = 0; x < cloneBitmap.Width; x++)
            {
                for (int y = 0; y < cloneBitmap.Height; y++)
                {
                    if (ignoreColors.Contains(cloneBitmap.GetPixel(x, y).ToArgb()) == false)
                    {
                        rValue = false;
                        if (outputFilePathOnCollision != null)
                            cloneBitmap.Save(outputFilePathOnCollision);
                        break;
                    }
                }
            }

            return rValue;
        }

        /// <summary>
        /// Draw a vertical line on the image (for debug purposes)
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="height"></param>
        /// <param name="line"></param>
        public static void Debug_DrawVerticalGuide(Graphics g, int x, int height, Pen line)
        {
            g.DrawLine(line, new Point(x, 0), new Point(x, height));
        }

        /// <summary>
        /// Draw a horizontal line on the image (for debug purposes)
        /// </summary>
        /// <param name="g"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="line"></param>
        public static void Debug_DrawHorizontalGuide(Graphics g, int y, int width, Pen line)
        {
            g.DrawLine(line, new Point(0, y), new Point(width, y));
        }

        public static void Debug_DrawPoint(Graphics g, int x, int y)
        {
            g.DrawEllipse(new Pen(Brushes.Yellow, 1), x, y, 3, 3);
        }

        /// <summary>
        /// Draw a rectangle (for debug purposes).
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        public static void Debug_DrawRectangle(Graphics g, Rectangle r, Pen p)
        {
            g.DrawRectangle(p, r);
        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                //DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit);
                
            }
            return bitmap;
        }

        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.SmoothingMode = SmoothingMode.AntiAlias;
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// Flip an image by the specified angle.
        /// The only supported angle at this stage is 90 'on-end'.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Bitmap FlipImage(Bitmap original, double angle)
        {
            Bitmap rValue;
            switch (angle)
            {
                case 90:
                    rValue = new Bitmap(original.Height, original.Width);

                    using (var g = Graphics.FromImage(original))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        for (int x = 0; x < original.Width; x++)
                        {
                            for (int y = 0; y < original.Height; y++)
                            {
                                Color c = original.GetPixel(x, y);
                                rValue.SetPixel(y, original.Width - x - 1, c);
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("Angle not supported by FlipImage");
            }

            return rValue;
        }
    }
}
