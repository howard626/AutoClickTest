using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace AutoClickTest
{
    public enum FishingState
    {
        Idle,
        WaitingForBite,
        Reeling,
        Minigame
    }

    public class FishingTestForm : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private FishingState currentState = FishingState.Idle;
        private DateTime lastStateChange = DateTime.Now;
        private Timer stateTimer;
        private bool isRunning = false;
        private Rectangle currentSearchRect = Screen.PrimaryScreen.Bounds;
        private double maxBiteSimSeen = 0;
        private double maxSpaceSimSeen = 0;
        private double maxFailSimSeen = 0;
        private bool isWaitingForNetCast = false;
        private bool isSingleMinigameMode = false;

        // UI Controls
        private Button btnStartStop;
        private Button btnStartMinigame;
        private Label lblState;
        private Label lblDetails;
        private TextBox txtLogs;
        private PictureBox picDebug;
        private CheckBox chkFocusWindow;

        // Image paths
        private string biteImagePath;
        private string spacebarImagePath;
        private string successImagePath;
        private string failImagePath;
        private string netIconImagePath;

        public FishingTestForm()
        {
            InitializePaths();
            InitializeComponent();
        }

        private void InitializePaths()
        {
            biteImagePath = GetImagePath("釣魚_驚嘆號.png");
            spacebarImagePath = GetImagePath("spacebar_template_2.png");
            successImagePath = GetImagePath("釣魚_灑網_成功.png");
            failImagePath = GetImagePath("釣魚_失敗.png");
            netIconImagePath = GetImagePath("釣魚_灑網圖示.png");
        }

        private string GetImagePath(string filename)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            
            string path = Path.Combine(baseDir, "img", filename);
            if (File.Exists(path)) return path;

            path = Path.Combine(baseDir, "..", "..", "..", "img", filename);
            if (File.Exists(path)) return Path.GetFullPath(path);

            path = Path.Combine(baseDir, "..", "..", "..", "..", "img", filename);
            if (File.Exists(path)) return Path.GetFullPath(path);

            path = Path.Combine(baseDir, "..", "..", "img", filename);
            if (File.Exists(path)) return Path.GetFullPath(path);

            path = Path.Combine(baseDir, filename);
            if (File.Exists(path)) return path;

            path = Path.Combine(Directory.GetCurrentDirectory(), "img", filename);
            if (File.Exists(path)) return path;

            return Path.Combine(baseDir, "img", filename);
        }

        private void InitializeComponent()
        {
            this.Size = new System.Drawing.Size(500, 700);
            this.Text = "方舟自動釣魚與灑網測試 (Lost Ark Auto Fishing)";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            btnStartStop = new Button() { Text = "開始自動釣魚", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(215, 40), Font = new Font("Arial", 12, FontStyle.Bold) };
            btnStartStop.Click += BtnStartStop_Click;

            btnStartMinigame = new Button() { Text = "單次灑網幫手", Location = new System.Drawing.Point(245, 20), Size = new System.Drawing.Size(215, 40), Font = new Font("Arial", 11, FontStyle.Bold) };
            btnStartMinigame.Click += BtnStartMinigame_Click;

            chkFocusWindow = new CheckBox() { Text = "開始時自動聚焦遊戲視窗 (Lost Ark)", Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(300, 20), Checked = true };

            lblState = new Label() { Text = "狀態: 停止", Location = new System.Drawing.Point(20, 100), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.Red };
            lblDetails = new Label() { Text = "資訊: --", Location = new System.Drawing.Point(20, 130), AutoSize = true, Font = new Font("Arial", 10) };

            txtLogs = new TextBox() { Location = new System.Drawing.Point(20, 160), Size = new System.Drawing.Size(440, 150), Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true, Font = new Font("Consolas", 9) };

            picDebug = new PictureBox() { Location = new System.Drawing.Point(20, 320), Size = new System.Drawing.Size(440, 320), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };

            this.Controls.AddRange(new Control[] { btnStartStop, btnStartMinigame, chkFocusWindow, lblState, lblDetails, txtLogs, picDebug });

            stateTimer = new Timer();
            stateTimer.Interval = 40; // 100ms interval for balanced responsiveness and CPU usage
            stateTimer.Tick += StateTimer_Tick;
        }

        private void Log(string message)
        {
            if (txtLogs.InvokeRequired)
            {
                txtLogs.Invoke(new Action<string>(Log), message);
                return;
            }
            txtLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void BtnStartMinigame_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                if (!File.Exists(spacebarImagePath))
                {
                    MessageBox.Show("找不到灑網圖片，無法啟動小遊戲幫手。");
                    return;
                }
                
                isWaitingForNetCast = true;
                isSingleMinigameMode = true;
                
                isRunning = true;
                btnStartStop.Text = "停止自動釣魚";
                btnStartStop.BackColor = Color.LightCoral;
                ChangeState(FishingState.Idle);
                stateTimer.Start();
                Log("小遊戲幫手啟動...");
            }
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                if (!File.Exists(biteImagePath))
                {
                    MessageBox.Show($"找不到驚嘆號圖檔：{biteImagePath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(spacebarImagePath))
                {
                    MessageBox.Show($"找不到灑網 Spacebar 模板圖檔：{spacebarImagePath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IntPtr gameHWnd = FindGameWindow();
                if (gameHWnd == IntPtr.Zero)
                {
                    Log("警告：找不到本機的 Lost Ark 視窗。因為您可能使用遠端桌面，請自行點擊遊戲畫面使其保持在最上層。");
                }
                else if (chkFocusWindow.Checked)
                {
                    SetForegroundWindow(gameHWnd);
                    System.Threading.Thread.Sleep(500);
                }

                isRunning = true;
                isWaitingForNetCast = false;
                isSingleMinigameMode = false;
                btnStartStop.Text = "停止自動釣魚";
                btnStartStop.BackColor = Color.LightCoral;
                ChangeState(FishingState.Idle);
                stateTimer.Start();
                Log("自動釣魚腳本啟動...");
            }
            else
            {
                isRunning = false;
                stateTimer.Stop();
                btnStartStop.Text = "開始自動釣魚";
                btnStartStop.BackColor = SystemColors.Control;
                lblState.Text = "狀態: 停止";
                lblState.ForeColor = Color.Red;
                lblDetails.Text = "資訊: --";
                Log("自動釣魚腳本停止。");
            }
        }

        private void ChangeState(FishingState newState)
        {
            currentState = newState;
            lastStateChange = DateTime.Now;
            if (newState == FishingState.WaitingForBite) 
            {
                maxBiteSimSeen = 0;
                maxSpaceSimSeen = 0;
                maxFailSimSeen = 0;
            }

            lblState.Text = $"狀態: {currentState}";
            switch (currentState)
            {
                case FishingState.Idle:
                    lblState.ForeColor = Color.Blue;
                    break;
                case FishingState.WaitingForBite:
                    lblState.ForeColor = Color.Orange;
                    break;
                case FishingState.Reeling:
                    lblState.ForeColor = Color.Purple;
                    break;
                case FishingState.Minigame:
                    lblState.ForeColor = Color.Green;
                    break;
            }
            Log($"切換狀態至: {currentState}");
        }

        private IntPtr FindGameWindow()
        {
            int currentId = Process.GetCurrentProcess().Id;
            var proc = Process.GetProcesses()
                .FirstOrDefault(p => p.Id != currentId && (p.ProcessName.ToLower().Contains("lostark") || p.MainWindowTitle.ToLower().Contains("lost ark")));
            return proc?.MainWindowHandle ?? IntPtr.Zero;
        }

        private Rectangle GetGameWindowRect()
        {
            IntPtr hWnd = FindGameWindow();
            if (hWnd != IntPtr.Zero)
            {
                RECT rect;
                if (GetWindowRect(hWnd, out rect))
                {
                    return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                }
            }
            return SystemInformation.VirtualScreen;
        }

        private Bitmap CaptureRegion(Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }

        private void StateTimer_Tick(object sender, EventArgs e)
        {
            if (!isRunning) return;

            switch (currentState)
            {
                case FishingState.Idle:
                    // Wait for 3 seconds to make sure animation is fully finished, then cast
                    double elapsedMs = (DateTime.Now - lastStateChange).TotalMilliseconds;
                    if (elapsedMs > 3000)
                    {
                        // 鎖定滑鼠目前所在的螢幕
                        currentSearchRect = Screen.FromPoint(Cursor.Position).Bounds;

                        bool isNetReady = false;
                        if (File.Exists(netIconImagePath))
                        {
                            using (Bitmap screenBmp = CaptureRegion(currentSearchRect))
                            using (Mat screenMat = BitmapToMat(screenBmp))
                            {
                                OpenCvSharp.Point? netLoc = MatchTemplateOnMat(screenMat, netIconImagePath, out double netSim);
                                // 當圖示為彩色時相似度會很高；如果是灰色冷卻狀態，相似度會大幅下降。我們設 0.75 作為判斷門檻。
                                if (netLoc.HasValue && netSim > 0.75) 
                                {
                                    isNetReady = true;
                                }
                            }
                        }

                        if (isWaitingForNetCast)
                        {
                            Log($"鎖定目標螢幕並強制模擬灑網 (按下 F)... 進入小遊戲偵測模式");
                            Task.Run(() => Keybord.Hold("F", 80));
                            isWaitingForNetCast = false;
                        }
                        else if (isNetReady)
                        {
                            Log($"[自動灑網] 偵測到灑網圖示亮起！鎖定目標螢幕並模擬灑網 (按下 F)...");
                            Task.Run(() => Keybord.Hold("F", 80));
                        }
                        else
                        {
                            Log($"鎖定目標螢幕並模擬拋竿 (按下 E)...");
                            Task.Run(() => Keybord.Hold("E", 80));
                        }
                        
                        ChangeState(FishingState.WaitingForBite);
                    }
                    else if (elapsedMs < 100 && (DateTime.Now - lastStateChange).TotalMilliseconds < 200)
                    {
                        // 剛進入 Idle，印出提示
                        if (isWaitingForNetCast)
                        {
                            Log("請在 3 秒內將滑鼠移至遊戲畫面，並『手動按下您的灑網快捷鍵』！");
                        }
                        else
                        {
                            Log("請在 3 秒內將滑鼠移至「遊戲所在的螢幕」並點擊，程式將自動鎖定該螢幕！");
                        }
                    }
                    break;

                case FishingState.WaitingForBite:
                    // Timeout check (35 seconds max for a bite)
                    if ((DateTime.Now - lastStateChange).TotalSeconds > 35)
                    {
                        Log($"[警告] 拋竿後 35 秒內無任何訊號。期間最高相似度: (驚嘆號 {maxBiteSimSeen:F2}, 失敗 {maxFailSimSeen:F2}, 灑網 {maxSpaceSimSeen:F2})。腳本停止。");
                        BtnStartStop_Click(null, null);
                        return;
                    }

                    double elapsedWaitMs = (DateTime.Now - lastStateChange).TotalMilliseconds;

                    // 強制等待 1.5 秒的拋竿/灑網動畫，這段期間內不做任何影像辨識，避免因為動畫特效導致誤判！
                    if (elapsedWaitMs < 1500)
                    {
                        Log($"等待動畫播完... ({(1500 - elapsedWaitMs) / 1000.0:F1}s)");
                        return;
                    }

                    // Perform screen search for Spacebar (QTE Minigame) and Exclamation Mark (!)
                    using (Bitmap screenBmp = CaptureRegion(currentSearchRect))
                    using (Mat screenMat = BitmapToMat(screenBmp))
                    {
                        // 防呆：將程式自己的視窗區域塗黑，避免 MatchTemplate 誤認自己的白色 UI 為遊戲畫面！
                        int x = Math.Max(0, this.Bounds.X - currentSearchRect.X);
                        int y = Math.Max(0, this.Bounds.Y - currentSearchRect.Y);
                        int w = Math.Min(this.Bounds.Width, screenMat.Width - x);
                        int h = Math.Min(this.Bounds.Height, screenMat.Height - y);
                        if (w > 0 && h > 0)
                        {
                            Cv2.Rectangle(screenMat, new OpenCvSharp.Rect(x, y, w, h), OpenCvSharp.Scalar.Black, -1);
                        }

                        // 1. Check if Net Casting Minigame is active (search for Spacebar prompt)
                        OpenCvSharp.Point? spaceLoc = MatchTemplateOnMat(screenMat, spacebarImagePath, out double spaceSim);
                        if (spaceSim > maxSpaceSimSeen) maxSpaceSimSeen = spaceSim;
                        
                        // 門檻恢復回 0.80，避免在小遊戲還沒跳出來之前，誤把白色的衣服或地板當成空白鍵！
                        if (spaceLoc.HasValue && spaceSim > 0.45)
                        {
                            Log($"偵測到灑網小遊戲出現！(相似度: {spaceSim:F2})");
                            ChangeState(FishingState.Minigame);
                            return;
                        }

                        if (!isSingleMinigameMode)
                        {
                            // 2. Check if Fish is biting (search for exclamation mark !)
                            OpenCvSharp.Point? biteLoc = MatchTemplateOnMat(screenMat, biteImagePath, out double biteSim);
                            if (biteSim > maxBiteSimSeen) maxBiteSimSeen = biteSim;
                            
                            // 3. Check for fishing fail message
                            double failSim = 0;
                            OpenCvSharp.Point? failLoc = null;
                            if (File.Exists(failImagePath))
                            {
                                failLoc = MatchTemplateOnMat(screenMat, failImagePath, out failSim);
                                if (failSim > maxFailSimSeen) maxFailSimSeen = failSim;
                            }

                            // 強制更新 UI (加上 Refresh 避免 UI 執行緒太忙導致畫面全白)
                            Log($"等待咬鉤中... (驚嘆號: {biteSim:F2}, 失敗訊息: {failSim:F2})");

                            // 門檻降至 0.55，並加入 4 秒冷卻時間。這樣即使浮標或水花有 0.64 的相似度，在前 4 秒也會被無視。
                            // 而真正的驚嘆號（剛剛測出約 0.60）在 4 秒後出現就能被準確抓到！
                            if (elapsedWaitMs > 4000 && biteLoc.HasValue && biteSim > 0.55)
                            {
                                Log($"魚咬鉤了！偵測到驚嘆號 (相似度: {biteSim:F2})，收竿！");
                                Task.Run(() => Keybord.Hold("E", 80));
                                ChangeState(FishingState.Reeling);
                                return;
                            }

                            // 安全網：將失敗門檻降至 0.56（因為平常最高 0.51）
                            if (failLoc.HasValue && failSim > 0.56)
                            {
                                Log($"[安全網] 漏抓了驚嘆號，但偵測到「釣魚失敗」(相似度: {failSim:F2})！重新重置釣魚狀態...");
                                ChangeState(FishingState.Reeling);
                            }
                        }
                        else
                        {
                            Log($"等待灑網小遊戲出現中...(相似度: {spaceSim:F2})");
                        }
                        
                        if (picDebug.Image != null) picDebug.Image.Dispose();
                        picDebug.Image = (Bitmap)screenBmp.Clone();
                        picDebug.Refresh(); 

                    }
                    break;

                case FishingState.Reeling:
                    // Wait for 4 seconds for the reeling animation to finish
                    if ((DateTime.Now - lastStateChange).TotalMilliseconds > 4000)
                    {
                        if (isSingleMinigameMode)
                        {
                            Log("單次灑網已完成，自動停止腳本。");
                            BtnStartStop_Click(null, null);
                        }
                        else
                        {
                            ChangeState(FishingState.Idle);
                        }
                    }
                    break;

                case FishingState.Minigame:
                    stateTimer.Stop();
                    Log(">>> 進入極速辨識模式 (UI 更新已暫停以提升效能) <<<");

                    bool minigameActive = true;
                    int loopCount = 0;

                    while (minigameActive && isRunning)
                    {
                        using (Bitmap screenBmp = CaptureRegion(currentSearchRect))
                        using (Mat screenMat = BitmapToMat(screenBmp))
                        {
                            // 1. 檢查 Spacebar 鎖定位置
                            OpenCvSharp.Point? spaceLoc = MatchTemplateOnMat(screenMat, spacebarImagePath, out double spaceSim);

                            if (!spaceLoc.HasValue || spaceSim < 0.40)
                            {
                                Log("小遊戲結束");
                                minigameActive = false;
                                break;
                            }

                            // 2. 定位 ROI
                            int roiX = Math.Max(0, spaceLoc.Value.X - 50);
                            Rect roiRect = new Rect(roiX, 0, Math.Min(550, screenMat.Width - roiX), screenMat.Height);

                            using (Mat subMat = new Mat(screenMat, roiRect))
                            {
                                // 3. 辨識 SUCCESS
                                OpenCvSharp.Point? successLoc = MatchTemplateOnMat(subMat, successImagePath, out double successSim);

                                // --- 效能優化：不要每一幀都 Log 和更新圖片 ---
                                loopCount++;
                                if (loopCount % 20 == 0) // 每 20 次辨識才更新一次 UI 數值
                                {
                                    lblDetails.Text = $"[運作中] Space:{spaceSim:F2} | SUCCESS:{successSim:F2}";

                                    // 每 20 次才畫一張圖，不然會太慢
                                    if (picDebug.Image != null) picDebug.Image.Dispose();
                                    picDebug.Image = MatToBitmap(subMat);
                                    picDebug.Refresh();
                                }

                                // 4. 觸發判斷
                                // 如果 0.75 還是不到，請先觀察 SUCCESS 出現時數值跳到多少
                                if (successLoc.HasValue && successSim > 0.70)
                                {
                                    Log($"[！！！成功！！！] 偵測到時機 (相似度: {successSim:F2})");

                                    // 連點 10 下
                                    for (int i = 0; i < 10; i++)
                                    {
                                        Keybord.Press("Space");
                                        System.Threading.Thread.Sleep(30);
                                    }

                                    minigameActive = false;
                                    break;
                                }
                            }
                        }

                        Application.DoEvents();
                        // 移除 Thread.Sleep(1)，追求極致速度
                    }

                    ChangeState(FishingState.Reeling);
                    stateTimer.Start();
                    break;
            }
        }

        private OpenCvSharp.Point? MatchTemplateOnMat(Mat src, string templatePath, out double maxVal)
        {
            maxVal = 0;
            if (!File.Exists(templatePath)) return null;

            using (Mat template = new Mat(templatePath))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(src, template, result, TemplateMatchModes.CCoeffNormed);
                double minVal;
                OpenCvSharp.Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
                
                return maxLoc;
            }
        }

        private static Mat BitmapToMat(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bytes = ms.ToArray();
                return Mat.FromImageData(bytes, ImreadModes.Color);
            }
        }

        private static Bitmap MatToBitmap(Mat mat)
        {
            byte[] bytes = mat.ToBytes(".bmp");
            using (var ms = new MemoryStream(bytes))
            {
                return new Bitmap(ms);
            }
        }
    }
}
