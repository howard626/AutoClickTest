using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AutoClickTest
{
    public partial class BackGround : Form
    {

        /// <summary>
        /// 獲取當前鼠標位置
        /// </summary>
        /// <param name="p"> 回傳的座標 </param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public extern static bool GetCursorPos(out Point p);

        public BackGround()
        {
            InitializeComponent();
        }

        public Point Point { get; set; }
        public bool IsSnipMode { get; set; } = false;
        public Bitmap CapturedImage { get; set; }
        
        private Point startPoint;
        private Rectangle selectionRect;
        private bool isSelecting = false;

        private void BackGround_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.MouseDown += BackGround_MouseDown;
            this.MouseMove += BackGround_MouseMove;
            this.MouseUp += BackGround_MouseUp;
        }

        private void BackGround_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                isSelecting = true;
            }
        }

        private void BackGround_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                int x = Math.Min(e.X, startPoint.X);
                int y = Math.Min(e.Y, startPoint.Y);
                int width = Math.Abs(e.X - startPoint.X);
                int height = Math.Abs(e.Y - startPoint.Y);
                selectionRect = new Rectangle(x, y, width, height);
                this.Invalidate();
            }
        }

        private void BackGround_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isSelecting) return;
            isSelecting = false;

            if (IsSnipMode)
            {
                if (selectionRect.Width > 5 && selectionRect.Height > 5)
                {
                    CaptureScreenArea(selectionRect);
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                GetCursorPos(out Point p);
                Point = p;
                this.DialogResult = DialogResult.OK;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (isSelecting && IsSnipMode)
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, selectionRect);
                }
            }
        }

        private void CaptureScreenArea(Rectangle rect)
        {
            // 隱藏視窗以擷取下方的內容
            this.Opacity = 0;
            System.Threading.Thread.Sleep(50); // 等視窗消失
            
            CapturedImage = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(CapturedImage))
            {
                g.CopyFromScreen(this.PointToScreen(rect.Location), Point.Empty, rect.Size);
            }
            
            this.Opacity = 0.5;
        }
    }
}
