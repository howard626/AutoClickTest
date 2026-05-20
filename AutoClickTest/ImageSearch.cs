using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenCvSharp;

namespace AutoClickTest
{
    public static class ImageSearch
    {
        // 手動將 Bitmap 轉換為 Mat，避免引入有衝突的 Extensions 套件
        private static Mat BitmapToMat(Bitmap bitmap)
        {
            BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                using (Mat mat = Mat.FromPixelData(bitmap.Height, bitmap.Width, MatType.CV_8UC4, bd.Scan0, bd.Stride))
                {
                    return mat.Clone();
                }
            }
            finally
            {
                bitmap.UnlockBits(bd);
            }
        }

        public static System.Drawing.Point? Find(string templatePath, Rectangle region, double threshold)
        {
            return Find(templatePath, region, threshold, out _, out _);
        }

        public static System.Drawing.Point? Find(string templatePath, Rectangle region, double threshold, out double maxSimilarity, out Rectangle matchRect)
        {
            maxSimilarity = 0;
            matchRect = Rectangle.Empty;

            if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath)) return null;

            try
            {
                if (region.Width <= 0 || region.Height <= 0)
                    region = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                // 擷取畫面
                using (Bitmap screen = new Bitmap(region.Width, region.Height))
                {
                    using (Graphics g = Graphics.FromImage(screen))
                    {
                        g.CopyFromScreen(region.Location, System.Drawing.Point.Empty, region.Size);
                    }

                    // 使用 OpenCV 進行進階特徵比對
                    using (Mat screenMat = BitmapToMat(screen))
                    using (Mat templateMat = Cv2.ImRead(templatePath, ImreadModes.Color)) // 讀取為彩色
                    {
                        if (screenMat.Empty() || templateMat.Empty()) return null;
                        
                        // 確保畫面比模板大
                        if (screenMat.Width < templateMat.Width || screenMat.Height < templateMat.Height)
                            return null;

                        // 統一色域 (螢幕擷取通常是 BGRA，需要轉成 BGR 來比對)
                        using (Mat screenBgr = new Mat())
                        {
                            if (screenMat.Channels() == 4)
                                Cv2.CvtColor(screenMat, screenBgr, ColorConversionCodes.BGRA2BGR);
                            else
                                screenMat.CopyTo(screenBgr);

                            using (Mat result = new Mat())
                            {
                                // CCoeffNormed 演算法：抗背景噪音能力強
                                Cv2.MatchTemplate(screenBgr, templateMat, result, TemplateMatchModes.CCoeffNormed);
                                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                                maxSimilarity = maxVal;
                                matchRect = new Rectangle(maxLoc.X, maxLoc.Y, templateMat.Width, templateMat.Height);

                                // 找到最佳匹配點，且相似度大於門檻值
                                if (maxVal >= threshold)
                                {
                                    // 回傳匹配圖案的「正中心點」，並加上 region.Location 的偏移量
                                    return new System.Drawing.Point(
                                        maxLoc.X + region.X + (templateMat.Width / 2),
                                        maxLoc.Y + region.Y + (templateMat.Height / 2)
                                    );
                                }
                            }
                        }
                    }
                }
            }
            catch 
            { 
                return null; 
            }
            
            return null;
        }
    }
}
