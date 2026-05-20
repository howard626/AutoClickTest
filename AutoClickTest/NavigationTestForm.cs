using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoClickTest
{
    public class NavigationTestForm : Form
    {
        private Label lblPlayerImage;
        private Label lblTargetImage;
        private Button btnStartStop;
        private Label lblDirection;
        private Label lblAngle;
        private CheckBox chkSimulateKeys;
        
        private string playerImagePath = "";
        private string targetImagePath = "";
        private Timer timer;
        private bool isTracking = false;
        private PictureBox picDebug;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private Rectangle GetGameWindowRect()
        {
            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainWindowTitle.Contains("FINAL FANTASY XIV"))
                    {
                        IntPtr hwnd = proc.MainWindowHandle;
                        if (hwnd != IntPtr.Zero)
                        {
                            RECT rect;
                            if (GetWindowRect(hwnd, out rect))
                            {
                                int width = rect.Right - rect.Left;
                                int height = rect.Bottom - rect.Top;
                                if (width > 100 && height > 100)
                                {
                                    return new Rectangle(rect.Left, rect.Top, width, height);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            return Rectangle.Empty;
        }

        public NavigationTestForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 650);
            this.Text = "全自動導航方位測試 (Auto Navigation)";

            // 1. 玩家中心圖示
            Label lblPlayerSetup = new Label() { Text = "1. 設定玩家小地圖圖示 (如: 藍色箭頭)", Location = new Point(20, 15), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            Button btnSnipPlayer = new Button() { Text = "截圖玩家 ✂️", Location = new Point(20, 40), Size = new Size(100, 30) };
            btnSnipPlayer.Click += (s, e) => SnipImage(ref playerImagePath, lblPlayerImage);
            lblPlayerImage = new Label() { Text = "尚未選擇", Location = new Point(130, 45), AutoSize = true };

            // 2. 目標圖示
            Label lblTargetSetup = new Label() { Text = "2. 設定追蹤目標圖示 (如: 驚嘆號)", Location = new Point(20, 85), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            Button btnSnipTarget = new Button() { Text = "截圖目標 ✂️", Location = new Point(20, 110), Size = new Size(100, 30) };
            btnSnipTarget.Click += (s, e) => SnipImage(ref targetImagePath, lblTargetImage);
            lblTargetImage = new Label() { Text = "尚未選擇", Location = new Point(130, 115), AutoSize = true };

            // 開始按鈕
            btnStartStop = new Button() { Text = "開始全自動追蹤", Location = new Point(20, 160), Size = new Size(330, 40), Font = new Font("Arial", 12, FontStyle.Bold) };
            btnStartStop.Click += BtnStartStop_Click;

            // 模擬按鍵核取方塊
            chkSimulateKeys = new CheckBox() { Text = "同步模擬鍵盤操作 (W/A/D 自動導航移動)", Location = new Point(20, 210), Size = new Size(330, 20), Checked = true };

            // 狀態文字
            lblDirection = new Label() { Text = "狀態: 等待開始...", Location = new Point(20, 240), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.Blue };
            lblAngle = new Label() { Text = "角度: --", Location = new Point(20, 270), AutoSize = true, Font = new Font("Arial", 10) };

            // 預覽圖
            picDebug = new PictureBox() { Location = new Point(20, 300), Size = new Size(330, 250), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };

            this.Controls.AddRange(new Control[] { 
                lblPlayerSetup, btnSnipPlayer, lblPlayerImage,
                lblTargetSetup, btnSnipTarget, lblTargetImage,
                btnStartStop, chkSimulateKeys, lblDirection, lblAngle, picDebug 
            });

            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
        }

        private void SnipImage(ref string imagePath, Label lbl)
        {
            using (BackGround b = new BackGround() { IsSnipMode = true })
            {
                if (b.ShowDialog(this) == DialogResult.OK && b.CapturedImage != null)
                {
                    string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    string fileName = $"nav_snip_{DateTime.Now:yyyyMMddHHmmss}.png";
                    imagePath = Path.Combine(dir, fileName);
                    b.CapturedImage.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                    lbl.Text = fileName;
                }
            }
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(targetImagePath) || !File.Exists(targetImagePath) ||
                string.IsNullOrEmpty(playerImagePath) || !File.Exists(playerImagePath))
            {
                MessageBox.Show("請先截圖設定好【玩家圖示】與【目標圖示】！");
                return;
            }

            isTracking = !isTracking;
            if (isTracking)
            {
                btnStartStop.Text = "停止追蹤";
                btnStartStop.BackColor = Color.LightCoral;
                timer.Start();
            }
            else
            {
                btnStartStop.Text = "開始全自動追蹤";
                btnStartStop.BackColor = SystemColors.Control;
                timer.Stop();
                lblDirection.Text = "狀態: 停止";
                lblDirection.ForeColor = Color.Black;
                lblAngle.Text = "角度: --";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 1. 取得 FF14 遊戲視窗範圍，只在遊戲範圍內尋找玩家 (避免被 VS Code 或是其他程式干擾)
            Rectangle gameRect = GetGameWindowRect();
            
            // 如果找不到遊戲視窗，就使用整個虛擬螢幕作為備用
            if (gameRect.IsEmpty)
            {
                gameRect = SystemInformation.VirtualScreen;
            }

            // 在遊戲範圍內尋找玩家中心點 (相似度稍微嚴格一點 0.8)
            Point? playerLoc = ImageSearch.Find(playerImagePath, gameRect, 0.8, out double playerMaxVal, out Rectangle playerRect);

            if (!playerLoc.HasValue)
            {
                lblDirection.Text = $"找不到玩家圖示 (最高相似度: {playerMaxVal:F2})";
                lblDirection.ForeColor = Color.Orange;
                lblAngle.Text = "請確認小地圖是否被遮住";
                if (picDebug.Image != null) picDebug.Image.Dispose();
                picDebug.Image = null;
                return;
            }

            int cx = playerLoc.Value.X;
            int cy = playerLoc.Value.Y;

            // 2. 找到玩家後，在玩家周圍 300x300 的範圍內尋找目標
            int mapSize = 300;
            Rectangle searchRegion = new Rectangle(
                Math.Max(0, cx - mapSize / 2), 
                Math.Max(0, cy - mapSize / 2), 
                mapSize, 
                mapSize
            );

            Bitmap debugBmp = new Bitmap(searchRegion.Width, searchRegion.Height);
            using (Graphics g = Graphics.FromImage(debugBmp))
            {
                g.CopyFromScreen(searchRegion.Location, Point.Empty, searchRegion.Size);
            }

            Point? targetLoc = ImageSearch.Find(targetImagePath, searchRegion, 0.75, out double targetMaxVal, out Rectangle matchRect);

            using (Graphics g = Graphics.FromImage(debugBmp))
            {
                // 畫出玩家中心點
                using (Pen pCenter = new Pen(Color.Blue, 2))
                {
                    g.DrawEllipse(pCenter, (mapSize/2) - 10, (mapSize/2) - 10, 20, 20);
                }
                
                // 畫出找到的目標紅框
                using (Pen pTarget = new Pen(Color.Red, 3))
                {
                    g.DrawRectangle(pTarget, matchRect);
                }
            }

            if (picDebug.Image != null) picDebug.Image.Dispose();
            picDebug.Image = debugBmp;

            if (targetLoc.HasValue)
            {
                int dx = targetLoc.Value.X - cx;
                int dy = targetLoc.Value.Y - cy;

                double angleRad = Math.Atan2(dy, dx);
                double angleDeg = angleRad * 180.0 / Math.PI;

                lblAngle.Text = $"角度: {angleDeg:F1}° (dx:{dx}, dy:{dy})";

                string direction = "";
                double forwardAngle = angleDeg + 90;
                if (forwardAngle > 180) forwardAngle -= 360;

                if (Math.Abs(forwardAngle) < 20)
                {
                    direction = "正前方 (按住 W)";
                    if (chkSimulateKeys.Checked)
                    {
                        Task.Run(() => Keybord.Hold("W", 350));
                    }
                }
                else if (forwardAngle > 0 && forwardAngle <= 135)
                {
                    direction = "偏右 (按一下 Right)";
                    if (chkSimulateKeys.Checked)
                    {
                        Task.Run(() => Keybord.Hold("Right", 100));
                    }
                }
                else if (forwardAngle < 0 && forwardAngle >= -135)
                {
                    direction = "偏左 (按一下 Left)";
                    if (chkSimulateKeys.Checked)
                    {
                        Task.Run(() => Keybord.Hold("Left", 100));
                    }
                }
                else
                {
                    direction = "在後方 (需要轉身)";
                    if (chkSimulateKeys.Checked)
                    {
                        Task.Run(() => Keybord.Hold("Right", 400));
                    }
                }

                lblDirection.Text = $"方向: {direction} (相似度: {targetMaxVal:F2})";
                lblDirection.ForeColor = Color.Green;
            }
            else
            {
                lblDirection.Text = $"找不到目標 (最高相似度: {targetMaxVal:F2})";
                lblDirection.ForeColor = Color.Red;
                lblAngle.Text = "角度: --";
            }
        }
    }
}
