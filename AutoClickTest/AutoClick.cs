using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AutoClickTest
{
    public partial class AutoClick : Form
    {
        /// <summary>
        /// 是否執行
        /// </summary>
        private bool IsStart = false;

        /// <summary>
        /// 動作列表
        /// </summary>
        private List<Action> Actions = new List<Action>();

        /// <summary>
        /// 動作記數
        /// </summary>
        private int ActionCount { get; set; }

        public AutoClick()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 載入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoClick_Load(object sender, EventArgs e)
        {
            InitializeActionBlocks();

            // 隱藏的導航測試按鈕
            System.Windows.Forms.Button btnNavTest = new System.Windows.Forms.Button();
            btnNavTest.Text = "導航方位測試";
            btnNavTest.Location = new System.Drawing.Point(1200, 90);
            btnNavTest.Size = new System.Drawing.Size(120, 29);
            btnNavTest.Click += (s, ev) => { new NavigationTestForm().Show(); };
            this.Controls.Add(btnNavTest);
        }

        /// <summary>
        /// 初始化動作積木
        /// </summary>
        private void InitializeActionBlocks()
        {
            AddActionBlock("鍵盤按一下", ActionNameId.鍵盤按一下, Color.LightBlue);
            AddActionBlock("鍵盤按住", ActionNameId.鍵盤按住, Color.LightCyan);
            AddActionBlock("點左鍵", ActionNameId.滑鼠點一下左鍵, Color.LightGreen);
            AddActionBlock("點兩下左鍵", ActionNameId.滑鼠點兩下左鍵, Color.LightGreen);
            AddActionBlock("點右鍵", ActionNameId.滑鼠點一下右鍵, Color.LightGreen);
            AddActionBlock("點中鍵", ActionNameId.滑鼠點一下中鍵, Color.LightGreen);
            AddActionBlock("前滾", ActionNameId.滑鼠前滾, Color.LightYellow);
            AddActionBlock("後滾", ActionNameId.滑鼠後滾, Color.LightYellow);
            AddActionBlock("圖案比對", ActionNameId.滑鼠圖案比對, Color.Plum);
            AddActionBlock("迴圈", ActionNameId.迴圈, Color.LightSalmon);

            this.ActionBlocksPanel.AutoScroll = true;
            this.ActionBlocksPanel.FlowDirection = FlowDirection.TopDown;
            this.ActionBlocksPanel.WrapContents = false;
            // 禁止水平捲動的終極手段：監聽大小變化並重置水平捲動
            this.ActionBlocksPanel.Scroll += (s, e) => {
                if (this.ActionBlocksPanel.HorizontalScroll.Value != 0)
                    this.ActionBlocksPanel.HorizontalScroll.Value = 0;
            };
        }

        private void AddActionBlock(string text, ActionNameId id, Color color)
        {
            FlowLayoutPanel block = new FlowLayoutPanel();
            block.AutoSize = true;
            block.FlowDirection = FlowDirection.LeftToRight;
            block.Margin = new Padding(3);
            block.Padding = new Padding(5);
            block.BackColor = color;
            block.Tag = id;
            block.BorderStyle = BorderStyle.FixedSingle;
            // 限制寬度並允許內部換行，這是消除水平捲軸的關鍵
            block.MaximumSize = new Size(this.ActionBlocksPanel.Width - 30, 0);
            block.WrapContents = true; 
            block.MouseDown += Block_MouseDown;
            block.MouseMove += Block_MouseMove;

            // 主要標題
            Label lblTitle = new Label() { Text = text, AutoSize = true, Margin = new Padding(0, 5, 0, 0), Font = new Font(this.Font, FontStyle.Bold) };
            lblTitle.MouseDown += Block_MouseDown;
            block.Controls.Add(lblTitle);

            // 依據動作類型加入不同的輸入框與輔助文字
            switch (id)
            {
                case ActionNameId.鍵盤按一下:
                case ActionNameId.鍵盤按住:
                    block.Controls.Add(new Label() { Text = "鍵:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
                    TextBox txtKey = new TextBox() { Name = "txtKey", Text = "A", Width = 50 }; // 稍微寬一點以顯示完整鍵名
                    txtKey.ReadOnly = true; // 設定唯讀，強制用按的
                    txtKey.BackColor = Color.White;
                    txtKey.KeyDown += (s, e) => {
                        txtKey.Text = e.KeyCode.ToString();
                        e.SuppressKeyPress = true; // 阻止原本按鍵字元輸入
                        e.Handled = true;
                    };
                    block.Controls.Add(txtKey);
                    if (id == ActionNameId.鍵盤按住)
                    {
                        block.Controls.Add(new Label() { Text = "按", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
                        TextBox txtHold = new TextBox() { Name = "txtHold", Text = "1000", Width = 45 };
                        txtHold.KeyPress += NumericOnly_KeyPress;
                        block.Controls.Add(txtHold);
                        block.Controls.Add(new Label() { Text = "ms", AutoSize = true, Margin = new Padding(0, 5, 0, 0) });
                    }
                    break;
                case ActionNameId.滑鼠前滾:
                case ActionNameId.滑鼠後滾:
                    TextBox txtTimes = new TextBox() { Name = "txtTimes", Text = "1", Width = 30 };
                    txtTimes.KeyPress += NumericOnly_KeyPress;
                    block.Controls.Add(txtTimes);
                    block.Controls.Add(new Label() { Text = "次", AutoSize = true, Margin = new Padding(0, 5, 0, 0) });
                    break;
                case ActionNameId.滑鼠點一下左鍵:
                case ActionNameId.滑鼠點兩下左鍵:
                case ActionNameId.滑鼠點一下右鍵:
                case ActionNameId.滑鼠點一下中鍵:
                    block.Controls.Add(new Label() { Text = "X:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
                    TextBox txtX = new TextBox() { Name = "txtX", Text = PointX.Text, Width = 35 };
                    txtX.KeyPress += NumericOnly_KeyPress;
                    block.Controls.Add(txtX);
                    block.Controls.Add(new Label() { Text = "Y:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
                    TextBox txtY = new TextBox() { Name = "txtY", Text = PointY.Text, Width = 35 };
                    txtY.KeyPress += NumericOnly_KeyPress;
                    block.Controls.Add(txtY);

                    // 增加「取得」座標按鈕
                    Button btnPick = new Button() { Text = "🎯", Width = 25, Height = 25, FlatStyle = FlatStyle.Flat, Margin = new Padding(3, 3, 0, 0) };
                    btnPick.Click += (s, e) => {
                        using (BackGround b = new BackGround())
                        {
                            if (b.ShowDialog(this) == DialogResult.OK)
                            {
                                txtX.Text = b.Point.X.ToString();
                                txtY.Text = b.Point.Y.ToString();
                            }
                        }
                    };
                    block.Controls.Add(btnPick);
                    break;
                case ActionNameId.滑鼠圖案比對:
                    block.Controls.Add(new Label() { Text = "圖案:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
                    Button btnUpload = new Button() { Text = "📤", Width = 30, Height = 25, FlatStyle = FlatStyle.Flat };
                    PictureBox picPreview = new PictureBox() { Width = 30, Height = 30, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
                    btnUpload.Click += (s, e) => {
                        using (OpenFileDialog ofd = new OpenFileDialog()) {
                            ofd.Filter = "Images|*.png;*.jpg;*.bmp";
                            if (ofd.ShowDialog() == DialogResult.OK) {
                                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                                string fileName = Path.GetFileName(ofd.FileName);
                                string targetPath = Path.Combine(dir, fileName);
                                File.Copy(ofd.FileName, targetPath, true);
                                btnUpload.Tag = targetPath;
                                picPreview.Image = Image.FromFile(targetPath);
                            }
                        }
                    };
                    block.Controls.Add(btnUpload);

                    Button btnSnip = new Button() { Text = "✂️", Width = 30, Height = 25, FlatStyle = FlatStyle.Flat };
                    btnSnip.Click += (s, e) => {
                        using (BackGround b = new BackGround() { IsSnipMode = true }) {
                            if (b.ShowDialog(this) == DialogResult.OK && b.CapturedImage != null) {
                                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                                string fileName = $"snip_{DateTime.Now:yyyyMMddHHmmss}.png";
                                string targetPath = Path.Combine(dir, fileName);
                                b.CapturedImage.Save(targetPath, ImageFormat.Png);
                                btnUpload.Tag = targetPath;
                                picPreview.Image = Image.FromFile(targetPath);
                            }
                        }
                    };
                    block.Controls.Add(btnSnip);
                    block.Controls.Add(picPreview);

                    Label lblExec = new Label() { Text = "執行:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) };
                    block.Controls.Add(lblExec);

                    ComboBox cbAction = new ComboBox() { Name = "cbMatchAction", Width = 80, DropDownStyle = ComboBoxStyle.DropDownList };
                    cbAction.Items.AddRange(new string[] { "左鍵點擊", "左鍵雙擊", "右鍵點擊", "僅移動", "鍵盤按鍵" });
                    cbAction.SelectedIndex = 0;
                    block.Controls.Add(cbAction);

                    // 動態區域：存放座標或按鍵
                    FlowLayoutPanel dynamicArea = new FlowLayoutPanel() { AutoSize = true, Margin = new Padding(0), WrapContents = false };
                    
                    Label lblX = new Label() { Text = "X:", AutoSize = true, Margin = new Padding(2, 5, 0, 0) };
                    TextBox txtImgX = new TextBox() { Name = "txtX", Text = "0", Width = 30 };
                    Label lblY = new Label() { Text = "Y:", AutoSize = true, Margin = new Padding(2, 5, 0, 0) };
                    TextBox txtImgY = new TextBox() { Name = "txtY", Text = "0", Width = 30 };
                    Button btnPickImg = new Button() { Text = "🎯", Width = 25, Height = 25, FlatStyle = FlatStyle.Flat, Margin = new Padding(2, 2, 0, 0) };
                    
                    TextBox txtKeyMatch = new TextBox() { Name = "txtKeyMatch", Width = 50, Visible = false, ReadOnly = true, BackColor = Color.White, Margin = new Padding(2, 5, 0, 0) };
                    txtKeyMatch.KeyDown += (s, e) => { txtKeyMatch.Text = e.KeyCode.ToString(); e.SuppressKeyPress = true; };

                    txtImgX.KeyPress += NumericOnly_KeyPress;
                    txtImgY.KeyPress += NumericOnly_KeyPress;
                    btnPickImg.Click += (s, e) => {
                        using (BackGround b = new BackGround()) {
                            if (b.ShowDialog(this) == DialogResult.OK) {
                                txtImgX.Text = b.Point.X.ToString();
                                txtImgY.Text = b.Point.Y.ToString();
                            }
                        }
                    };

                    dynamicArea.Controls.Add(lblX);
                    dynamicArea.Controls.Add(txtImgX);
                    dynamicArea.Controls.Add(lblY);
                    dynamicArea.Controls.Add(txtImgY);
                    dynamicArea.Controls.Add(btnPickImg);
                    dynamicArea.Controls.Add(txtKeyMatch);
                    block.Controls.Add(dynamicArea);

                    cbAction.SelectedIndexChanged += (s, e) => {
                        block.SuspendLayout();
                        bool isKeyboard = (cbAction.Text == "鍵盤按鍵");
                        txtKeyMatch.Visible = isKeyboard;
                        lblX.Visible = txtImgX.Visible = lblY.Visible = txtImgY.Visible = btnPickImg.Visible = !isKeyboard;
                        block.ResumeLayout();
                    };

                    CheckBox chkComplex = new CheckBox() { Name = "chkComplex", Text = "IF-ELSE 模式", AutoSize = true, Margin = new Padding(5, 5, 0, 0) };
                    chkComplex.CheckedChanged += (s, e) => {
                        block.SuspendLayout();
                        bool complex = chkComplex.Checked;
                        lblExec.Visible = cbAction.Visible = !complex;
                        dynamicArea.Visible = !complex && (cbAction.Text != "鍵盤按鍵");
                        txtKeyMatch.Visible = !complex && (cbAction.Text == "鍵盤按鍵");
                        block.ResumeLayout();
                    };
                    block.Controls.Add(chkComplex);
                    break;
                case ActionNameId.迴圈:
                    block.Controls.Add(new Label() { Text = "次數:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
                    TextBox txtLoopCount = new TextBox() { Name = "txtLoopCount", Text = "5", Width = 40 };
                    txtLoopCount.KeyPress += NumericOnly_KeyPress;
                    block.Controls.Add(txtLoopCount);
                    block.Controls.Add(new Label() { Text = "次 (0為無限)", AutoSize = true, Margin = new Padding(0, 5, 0, 0) });
                    break;
                    break;
            }

            // 所有動作都要有延遲設定
            block.Controls.Add(new Label() { Text = "延遲:", AutoSize = true, Margin = new Padding(5, 5, 0, 0) });
            TextBox txtDelay = new TextBox() { Name = "txtDelay", Text = "500", Width = 40 };
            txtDelay.KeyPress += NumericOnly_KeyPress;
            block.Controls.Add(txtDelay);
            block.Controls.Add(new Label() { Text = "ms", AutoSize = true, Margin = new Padding(0, 5, 0, 0) });

            // 將所有子控制項的 MouseDown 也導向 Block_MouseDown (除了 TextBox)
            foreach (Control ctrl in block.Controls)
            {
                if (!(ctrl is TextBox) && !(ctrl is Button))
                {
                    ctrl.MouseDown += (s, ev) => Block_MouseDown(block, ev);
                    ctrl.MouseMove += (s, ev) => Block_MouseMove(block, ev);
                }
            }

            ActionBlocksPanel.Controls.Add(block);
        }

        private System.Drawing.Point dragStartPoint;
        private void Block_MouseDown(object sender, MouseEventArgs e)
        {
            dragStartPoint = e.Location;
        }

        private void Block_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (dragStartPoint == System.Drawing.Point.Empty) return;

            // 檢查移動距離是否足以觸發拖曳 (避免誤觸導致子按鈕無法點擊)
            if (Math.Abs(e.X - dragStartPoint.X) > SystemInformation.DragSize.Width ||
                Math.Abs(e.Y - dragStartPoint.Y) > SystemInformation.DragSize.Height)
            {
                Control block = (Control)sender;
                if (block.Tag is ActionNameId id)
                {
                    var dragData = new ActionDragData {
                        Id = id,
                        Values = new Dictionary<string, string>()
                    };
                    foreach (Control ctrl in block.Controls)
                    {
                        if (ctrl is Button btn && btn.Text == "📤" && btn.Tag != null) dragData.Values["txtImg"] = btn.Tag.ToString();
                        if (ctrl is ComboBox cb) dragData.Values[cb.Name] = cb.Text;
                        if (ctrl is TextBox txt) dragData.Values[txt.Name] = txt.Text;
                    }
                    block.DoDragDrop(dragData, DragDropEffects.Copy);
                }
                dragStartPoint = System.Drawing.Point.Empty; // 重置
            }
        }

        public class ActionDragData
        {
            public ActionNameId Id { get; set; }
            public Dictionary<string, string> Values { get; set; }
        }

        private void ActionList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ActionDragData)) || e.Data.GetDataPresent(typeof(Panel)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ActionList_DragDrop(object sender, DragEventArgs e)
        {
            // 取得落點位置
            System.Drawing.Point targetPoint = ActionListPanel.PointToClient(new System.Drawing.Point(e.X, e.Y));
            Control targetCtrl = ActionListPanel.GetChildAtPoint(targetPoint);
            int insertIndex = targetCtrl != null ? ActionListPanel.Controls.GetChildIndex(targetCtrl) : ActionListPanel.Controls.Count;

            if (e.Data.GetDataPresent(typeof(ActionDragData)))
            {
                ActionDragData data = (ActionDragData)e.Data.GetData(typeof(ActionDragData));
                AddActionWithData(data, insertIndex);
            }
            else if (e.Data.GetDataPresent(typeof(Panel))) // 支援拖拽排序
            {
                Panel draggedPanel = (Panel)e.Data.GetData(typeof(Panel));
                if (draggedPanel != null && targetCtrl != draggedPanel)
                {
                    int oldIndex = ActionListPanel.Controls.GetChildIndex(draggedPanel);
                    Action action = Actions[oldIndex];

                    Actions.RemoveAt(oldIndex);
                    ActionListPanel.Controls.RemoveAt(oldIndex);

                    if (insertIndex > oldIndex && insertIndex > 0) insertIndex--;

                    Actions.Insert(insertIndex, action);
                    ActionListPanel.Controls.Add(draggedPanel);
                    ActionListPanel.Controls.SetChildIndex(draggedPanel, insertIndex);
                }
            }
        }

        private Action CreateActionFromData(ActionDragData data)
        {
            Action action = null;
            int delay = data.Values.ContainsKey("txtDelay") ? (int.TryParse(data.Values["txtDelay"], out int d) ? d : 500) : 500;
            
            string key = data.Values.ContainsKey("txtKey") ? data.Values["txtKey"] : "A";
            int hold = data.Values.ContainsKey("txtHold") ? (int.TryParse(data.Values["txtHold"], out int h) ? h : 1000) : 1000;
            int x = data.Values.ContainsKey("txtX") ? (int.TryParse(data.Values["txtX"], out int dx) ? dx : 0) : 0;
            int y = data.Values.ContainsKey("txtY") ? (int.TryParse(data.Values["txtY"], out int dy) ? dy : 0) : 0;
            int times = data.Values.ContainsKey("txtTimes") ? (int.TryParse(data.Values["txtTimes"], out int dt) ? dt : 1) : 1;
            string imgPath = data.Values.ContainsKey("txtImg") ? data.Values["txtImg"] : "";

            switch (data.Id)
            {
                case ActionNameId.鍵盤按一下:
                    action = new KeyCodeAction() { Tool_Name = "鍵盤", Action_Desc = "按一下", KeyCode = key, Delay_MS = delay };
                    break;
                case ActionNameId.鍵盤按住:
                    action = new KeyCodeAction() { Tool_Name = "鍵盤", Action_Desc = "按住", KeyCode = key, Hold_MS = hold, Delay_MS = delay };
                    break;
                case ActionNameId.滑鼠點一下左鍵:
                case ActionNameId.滑鼠點兩下左鍵:
                case ActionNameId.滑鼠點一下右鍵:
                case ActionNameId.滑鼠點一下中鍵:
                    string desc = data.Id.ToString().Replace("滑鼠", "");
                    action = new MouseAction() { Tool_Name = "滑鼠", Action_Desc = desc, X = x, Y = y, Delay_MS = delay };
                    break;
                case ActionNameId.滑鼠前滾:
                case ActionNameId.滑鼠後滾:
                    string rollDesc = data.Id.ToString().Replace("滑鼠", "");
                    action = new MouseAction() { Tool_Name = "滑鼠", Action_Desc = rollDesc, X = times, Delay_MS = delay };
                    break;
                case ActionNameId.滑鼠圖案比對:
                    string mAction = data.Values.ContainsKey("cbMatchAction") ? data.Values["cbMatchAction"] : "左鍵點擊";
                    string mKey = data.Values.ContainsKey("txtKeyMatch") ? data.Values["txtKeyMatch"] : "";
                    bool isComplex = data.Values.ContainsKey("chkComplex") && data.Values["chkComplex"] == "True";
                    action = new ImageAction() { 
                        Tool_Name = "滑鼠", 
                        Action_Desc = "圖案比對", 
                        ImagePath = imgPath, 
                        Delay_MS = delay,
                        MatchActionType = mAction,
                        KeyCode = mKey,
                        X = x,
                        Y = y,
                        IsComplexMode = isComplex
                    };
                    break;
                case ActionNameId.迴圈:
                    int lCount = data.Values.ContainsKey("txtLoopCount") ? (int.TryParse(data.Values["txtLoopCount"], out int lc) ? lc : 5) : 5;
                    action = new LoopAction() { Tool_Name = "控制", Action_Desc = "迴圈", LoopCount = lCount, Delay_MS = delay };
                    break;
            }
            return action;
        }

        private void AddActionWithData(ActionDragData data, int index)
        {
            Action action = CreateActionFromData(data);
            if (action != null)
            {
                action.ID = ActionCount++;
                Actions.Insert(index, action);
                Panel block = CreateActionBlock(action);
                ActionListPanel.Controls.Add(block);
                ActionListPanel.Controls.SetChildIndex(block, index);
                txtResult.Text = "Added";
            }
        }

        private Panel CreateActionBlock(Action action)
        {
            FlowLayoutPanel block = new FlowLayoutPanel();
            block.AutoSize = true;
            block.FlowDirection = FlowDirection.LeftToRight;
            block.Padding = new Padding(5);
            block.Margin = new Padding(3);
            block.BorderStyle = BorderStyle.FixedSingle;
            block.BackColor = Color.WhiteSmoke;
            block.WrapContents = false;
            block.MouseDown += (s, e) => block.DoDragDrop(block, DragDropEffects.Move);

            Label lbl = new Label() { AutoSize = true, Margin = new Padding(0, 5, 0, 0) };
            UpdateActionLabelText(lbl, action);
            block.Controls.Add(lbl);

            Button btnDel = new Button() { Text = "X", Width = 25, Height = 25, ForeColor = Color.Red, FlatStyle = FlatStyle.Flat };
            btnDel.Click += (s, e) => {
                // 如果是在巢狀結構內，需要特殊處理
                if (block.Parent is FlowLayoutPanel parentSlot && parentSlot.Tag is List<Action> subList)
                {
                    int sidx = parentSlot.Controls.GetChildIndex(block);
                    subList.RemoveAt(sidx);
                    parentSlot.Controls.Remove(block);
                }
                else
                {
                    int idx = ActionListPanel.Controls.GetChildIndex(block);
                    Actions.RemoveAt(idx);
                    ActionListPanel.Controls.Remove(block);
                }
            };
            block.Controls.Add(btnDel);

            lbl.DoubleClick += (s, e) => ActionList_NodeMouseDoubleClick(block);

            // --- IF-ELSE 模式特殊處理 ---
            if (action is ImageAction img && img.IsComplexMode)
            {
                block.FlowDirection = FlowDirection.TopDown;
                
                block.Controls.Add(new Label() { Text = "┌─ 如果找到圖案：", AutoSize = true, Margin = new Padding(15, 5, 0, 0), ForeColor = Color.DarkGreen });
                FlowLayoutPanel successSlot = CreateSlot(img.SuccessActions);
                block.Controls.Add(successSlot);

                block.Controls.Add(new Label() { Text = "├─ 否則 (ELSE)：", AutoSize = true, Margin = new Padding(15, 5, 0, 0), ForeColor = Color.DarkRed });
                FlowLayoutPanel failureSlot = CreateSlot(img.FailureActions);
                block.Controls.Add(failureSlot);
                
                block.Controls.Add(new Label() { Text = "└─ 結束判斷", AutoSize = true, Margin = new Padding(15, 0, 0, 5) });
            }
            else if (action is LoopAction loop)
            {
                block.FlowDirection = FlowDirection.TopDown;
                block.Controls.Add(new Label() { Text = "┌─ 迴圈開始：", AutoSize = true, Margin = new Padding(15, 5, 0, 0), ForeColor = Color.SaddleBrown });
                FlowLayoutPanel loopSlot = CreateSlot(loop.LoopActions);
                block.Controls.Add(loopSlot);
                block.Controls.Add(new Label() { Text = "└─ 迴圈結束", AutoSize = true, Margin = new Padding(15, 0, 0, 5) });
            }

            return block;
        }

        private FlowLayoutPanel CreateSlot(List<Action> subList)
        {
            FlowLayoutPanel slot = new FlowLayoutPanel()
            {
                AutoSize = true,
                MinimumSize = new Size(350, 40),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 240, 240),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AllowDrop = true,
                Margin = new Padding(30, 0, 0, 5),
                Tag = subList 
            };
            slot.DragEnter += ActionList_DragEnter;
            slot.DragDrop += (s, e) => {
                Point targetPoint = slot.PointToClient(new Point(e.X, e.Y));
                Control targetCtrl = slot.GetChildAtPoint(targetPoint);
                int insertIndex = targetCtrl != null ? slot.Controls.GetChildIndex(targetCtrl) : slot.Controls.Count;

                if (e.Data.GetDataPresent(typeof(ActionDragData)))
                {
                    ActionDragData data = (ActionDragData)e.Data.GetData(typeof(ActionDragData));
                    Action act = CreateActionFromData(data);
                    if (act != null)
                    {
                        act.ID = ActionCount++;
                        subList.Insert(insertIndex, act);
                        Panel subBlock = CreateActionBlock(act);
                        slot.Controls.Add(subBlock);
                        slot.Controls.SetChildIndex(subBlock, insertIndex);
                    }
                }
            };
            
            foreach (var act in subList)
                slot.Controls.Add(CreateActionBlock(act));
                
            return slot;
        }

        private void UpdateActionLabelText(Label lbl, Action action)
        {
            Type type = action.GetType();
            if (type == typeof(KeyCodeAction))
            {
                KeyCodeAction key = (KeyCodeAction)action;
                string holdText = key.Action_Desc == "按住" ? $" 按住 {key.Hold_MS} 毫秒" : "";
                lbl.Text = $"{key.Action_Desc} {key.KeyCode} 鍵{holdText}--延遲 {key.Delay_MS} 毫秒";
            }
            else if (type == typeof(MouseAction))
            {
                MouseAction mouse = (MouseAction)action;
                if (mouse.Action_Desc.Contains("滾"))
                    lbl.Text = $"滑鼠 {mouse.Action_Desc} {mouse.X} 次--延遲 {mouse.Delay_MS} 毫秒";
                else
                    lbl.Text = $"在 {mouse.X},{mouse.Y} {mouse.Action_Desc}--延遲 {mouse.Delay_MS} 毫秒";
            }
            else if (type == typeof(ImageAction))
            {
                ImageAction img = (ImageAction)action;
                string coordInfo = (img.X != 0 || img.Y != 0) ? $" 在 {img.X},{img.Y}" : " 在圖案中心";
                lbl.Text = $"當看到 {Path.GetFileName(img.ImagePath)} 執行 {img.MatchActionType}{coordInfo}{(img.MatchActionType == "鍵盤按鍵" ? " " + img.KeyCode : "")}--延遲 {img.Delay_MS} 毫秒";
            }
            else if (type == typeof(LoopAction))
            {
                LoopAction loop = (LoopAction)action;
                string countText = loop.LoopCount == 0 ? "無限" : loop.LoopCount.ToString();
                lbl.Text = $"迴圈執行 {countText} 次--延遲 {loop.Delay_MS} 毫秒";
            }
        }
        /// <summary>
        /// 增加排程動作 (彈出視窗)
        /// </summary>
        private void AddAction(object sender, EventArgs e)
        {
            using (AddForm addDialog = new AddForm())
            {
                if (addDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Action action = addDialog.Action;
                    action.ID = ActionCount++;
                    Actions.Add(action);
                    
                    Panel block = CreateActionBlock(action);
                    ActionListPanel.Controls.Add(block);
                    txtResult.Text = "Ok";
                }
            }
        }

        /// <summary>
        /// 點擊編輯動作
        /// </summary>
        private void ActionList_NodeMouseDoubleClick(Control block)
        {
            int index = ActionListPanel.Controls.GetChildIndex(block);
            Action action = Actions[index];
            AddForm editDialog;

            if (action is KeyCodeAction)
                editDialog = new AddForm((KeyCodeAction)action);
            else if (action is MouseAction)
                editDialog = new AddForm((MouseAction)action);
            else
                editDialog = new AddForm((ImageAction)action);

            if (editDialog.ShowDialog(this) == DialogResult.OK)
            {
                Actions[index] = editDialog.Action;
                UpdateActionLabelText((Label)block.Controls[0], Actions[index]);
                txtResult.Text = "Updated";
            }
            editDialog.Dispose();
        }

        /// <summary>
        /// 開始按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, EventArgs e)
        {
            IsStart = true;
            Run();
        }
        
        /// <summary>
        /// 開始執行程序
        /// </summary>
        private async void Run()
        {
            await ExecuteList(Actions);
        }

        private async Task ExecuteList(List<Action> actionList)
        {
            foreach (var action in actionList)
            {
                if (!IsStart) break;
                await Task.Delay(action.Delay_MS);
                await RunSingleAction(action);
            }
        }

        private async Task RunSingleAction(Action action)
        {
            if (!IsStart) return;
            Random random = new Random();

            if (action is KeyCodeAction key)
            {
                switch (key.Action_Desc)
                {
                    case "按一下": Keybord.Press(key.KeyCode.ToUpper()); break;
                    case "按住": Keybord.Hold(key.KeyCode.ToUpper(), key.Hold_MS); break;
                }
            }
            else if (action is MouseAction mouse && !(action is ImageAction))
            {
                if (mouse.Action_Desc.Contains("滾"))
                {
                    for (int j = 0; j < mouse.X; j++) 
                        Mouse.Roll(mouse.Action_Desc == "前滾" ? Mouse.ROLLWAY.FRONT : Mouse.ROLLWAY.BACK);
                }
                else
                {
                    Mouse.Move(mouse.X + random.Next(-5, 5), mouse.Y + random.Next(-5, 5));
                    switch (mouse.Action_Desc)
                    {
                        case "點一下左鍵": Mouse.LeftClick(); break;
                        case "點兩下左鍵": Mouse.LeftClick(); Thread.Sleep(50); Mouse.LeftClick(); break;
                        case "點一下右鍵": Mouse.RightClick(); break;
                        case "點一下中鍵": Mouse.MiddleClick(); break;
                    }
                }
            }
            else if (action is ImageAction img)
            {
                Point? loc = ImageSearch.Find(img.ImagePath, img.SearchRegion, img.Threshold);
                if (loc.HasValue)
                {
                    if (img.IsComplexMode)
                    {
                        await ExecuteList(img.SuccessActions);
                    }
                    else
                    {
                        int targetX = (img.X != 0 || img.Y != 0) ? img.X : loc.Value.X;
                        int targetY = (img.X != 0 || img.Y != 0) ? img.Y : loc.Value.Y;
                        Mouse.Move(targetX, targetY);
                        switch (img.MatchActionType)
                        {
                            case "左鍵點擊": Mouse.LeftClick(); break;
                            case "左鍵雙擊": Mouse.LeftClick(); Thread.Sleep(50); Mouse.LeftClick(); break;
                            case "右鍵點擊": Mouse.RightClick(); break;
                            case "鍵盤按鍵": Keybord.Press(img.KeyCode); break;
                        }
                    }
                }
                else if (img.IsComplexMode)
                {
                    await ExecuteList(img.FailureActions);
                }
            }
            else if (action is LoopAction loop)
            {
                int count = loop.LoopCount;
                if (count == 0) // Infinite loop
                {
                    while (IsStart)
                    {
                        await ExecuteList(loop.LoopActions);
                        await Task.Delay(10); // Small delay to prevent tight loop
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (!IsStart) break;
                        await ExecuteList(loop.LoopActions);
                    }
                }
            }
        }

        /// <summary>
        /// 停止按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, EventArgs e)
        {
            IsStart = false;
        }

        private void Test_Click(object sender, EventArgs e)
        {
            BackGround b = new BackGround();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (b.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                txtResult.Text = $"{b.Point.X}, {b.Point.Y}";
            }
            else
            {
                txtResult.Text = "Failed";
            }
            b.Dispose();
        }

        private void MoveMouse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PointX.Text) || string.IsNullOrWhiteSpace(PointY.Text))
            { }
            else
            {
                Mouse.Move(int.Parse(PointX.Text ?? "0"), int.Parse(PointY.Text ?? "0"));
            }
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 清除按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, EventArgs e)
        {
            IsStart = false;
            Actions.Clear();
            ActionListPanel.Controls.Clear();
        }
    }
}
