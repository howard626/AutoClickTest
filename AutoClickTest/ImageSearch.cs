using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AutoClickTest
{
    public static class ImageSearch
    {
        public static Point? Find(string templatePath, Rectangle region, double threshold)
        {
            if (string.IsNullOrEmpty(templatePath) || !System.IO.File.Exists(templatePath)) return null;

            try
            {
                using (Bitmap template = new Bitmap(templatePath))
                {
                    if (region.Width <= 0 || region.Height <= 0)
                        region = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                    using (Bitmap screen = new Bitmap(region.Width, region.Height))
                    {
                        using (Graphics g = Graphics.FromImage(screen))
                        {
                            g.CopyFromScreen(region.Location, Point.Empty, region.Size);
                        }

                        return FindMatch(screen, template, threshold, region.Location);
                    }
                }
            }
            catch { return null; }
        }

        private static Point? FindMatch(Bitmap screen, Bitmap template, double threshold, Point offset)
        {
            int sw = screen.Width;
            int sh = screen.Height;
            int tw = template.Width;
            int th = template.Height;

            BitmapData screenData = screen.LockBits(new Rectangle(0, 0, sw, sh), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData templateData = template.LockBits(new Rectangle(0, 0, tw, th), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                byte[] screenBytes = new byte[screenData.Stride * sh];
                byte[] templateBytes = new byte[templateData.Stride * th];

                Marshal.Copy(screenData.Scan0, screenBytes, 0, screenBytes.Length);
                Marshal.Copy(templateData.Scan0, templateBytes, 0, templateBytes.Length);

                for (int y = 0; y <= sh - th; y += 2)
                {
                    for (int x = 0; x <= sw - tw; x += 2)
                    {
                        if (IsMatch(screenBytes, screenData.Stride, templateBytes, templateData.Stride, x, y, tw, th, threshold))
                        {
                            return new Point(x + offset.X + tw / 2, y + offset.Y + th / 2);
                        }
                    }
                }
            }
            finally
            {
                screen.UnlockBits(screenData);
                template.UnlockBits(templateData);
            }

            return null;
        }

        private static bool IsMatch(byte[] screen, int sStride, byte[] template, int tStride, int startX, int startY, int tw, int th, double threshold)
        {
            int matchCount = 0;
            int totalChecked = 0;

            for (int ty = 0; ty < th; ty += 2)
            {
                for (int tx = 0; tx < tw; tx += 2)
                {
                    int sPos = (startY + ty) * sStride + (startX + tx) * 4;
                    int tPos = ty * tStride + tx * 4;

                    byte sB = screen[sPos];
                    byte sG = screen[sPos + 1];
                    byte sR = screen[sPos + 2];

                    byte tB = template[tPos];
                    byte tG = template[tPos + 1];
                    byte tR = template[tPos + 2];

                    if (Math.Abs(sR - tR) < 30 && Math.Abs(sG - tG) < 30 && Math.Abs(sB - tB) < 30)
                    {
                        matchCount++;
                    }
                    totalChecked++;
                }
            }

            return (double)matchCount / totalChecked >= threshold;
        }
    }
}
