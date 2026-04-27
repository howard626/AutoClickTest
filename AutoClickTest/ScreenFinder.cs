using System;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace AutoClickTest
{
    public static class ScreenFinder
    {
        // 設定應用程式為 DPI 感知
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        static ScreenFinder()
        {
            try { SetProcessDPIAware(); }
            catch { }
        }

        /// <summary>
        /// 擷取當前螢幕並尋找目標圖示位置
        /// </summary>
        /// <param name="templatePath">目標圖示（如 "target.png"）</param>
        /// <param name="confidence">信心閾值 0~1（推薦0.8）</param>
        /// <returns>找到則 (centerX, centerY)；找不到回傳 null</returns>
        public static (int x, int y)? FindImageOnScreen(string templatePath, double confidence = 0.8)
        {
            using var bmp = CaptureScreenBitmap();
            using var srcMat = BitmapConverter.ToMat(bmp);
            using var template = Cv2.ImRead(templatePath, ImreadModes.Color);

            if (template.Width > srcMat.Width || template.Height > srcMat.Height)
                throw new ArgumentException("範本圖尺寸比螢幕還大");

            using var result = new Mat();
            Cv2.MatchTemplate(srcMat, template, result, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal >= confidence)
            {
                int centerX = maxLoc.X + template.Width / 2;
                int centerY = maxLoc.Y + template.Height / 2;

                // ← 你可以在這裡呼叫你的模擬點擊、鍵盤等函式
                // 例如：Mouse.SetCursorPos(centerX, centerY);

                return (centerX, centerY);
            }
            return null;
        }

        private static Bitmap CaptureScreenBitmap()
        {
            var scr = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            var bmp = new Bitmap(scr.Width, scr.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(scr.Left, scr.Top, 0, 0, scr.Size, CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }
    }
}