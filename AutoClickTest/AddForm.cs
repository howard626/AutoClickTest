using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoClickTest
{
    public partial class AddForm : Form
    {
        /// <summary>
        /// 新增動作視窗
        /// </summary>
        public AddForm()
        {
            InitializeComponent();
            Ok.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
            Init();
        }

        /// <summary>
        /// 新增動作視窗
        /// </summary>
        public AddForm(MouseAction action)
        {
            InitializeComponent();
            Ok.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
            Init();
            Action = action;
            switch (Action.Action_Desc)
            {
                case "點一下左鍵":
                case "點兩下左鍵":
                case "點一下右鍵":
                case "點一下中鍵":
                    ShowMouseClick();
                    break;
                case "前滾":
                case "後滾":
                    ShowMouseRoll();
                    break;
            }
        }

        /// <summary>
        /// 新增動作視窗
        /// </summary>
        public AddForm(KeyCodeAction action)
        {
            InitializeComponent();
            Ok.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
            Init();
            Action = action;
            ShowKeyCode();
        }



        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            if (Tool.Items.Count == 0)
            {
                Tool.Items.Insert(0, "鍵盤");
                Tool.Items.Insert(1, "滑鼠");
            }
            HideMouse();
            HideKeyCode();
        }

        /// <summary>
        /// 要新增的動作
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// 工具選擇事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Tool.Text)
            {
                case "鍵盤":
                    ActionName.Items.Clear();
                    ActionName.Items.Add("按一下");
                    HideMouse();
                    ShowKeyCode();
                    break;
                case "滑鼠":
                    ActionName.Items.Clear();
                    ActionName.Items.Add("點一下左鍵");
                    ActionName.Items.Add("點兩下左鍵");
                    ActionName.Items.Add("點一下右鍵");
                    ActionName.Items.Add("點一下中鍵");
                    ActionName.Items.Add("前滾");
                    ActionName.Items.Add("後滾");
                    HideKeyCode();
                    ShowMouseClick();
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 確認按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Click(object sender, EventArgs e)
        {
            if (Tool.Text == "鍵盤")
            {
                Action = new KeyCodeAction()
                {
                    Tool_Name = "鍵盤",
                    Action_Desc = ActionName.Text,
                    Delay_MS = Int32.Parse(CheckStringToInt(Delay.Text)),
                    KeyCode = KeyCode.Text
                };
            }
            else {
                switch (ActionName.Text)
                {
                    case "點一下左鍵":
                    case "點兩下左鍵":
                    case "點一下右鍵":
                    case "點一下中鍵":
                        Action = new MouseAction()
                        {
                            Tool_Name = "滑鼠",
                            Action_Desc = ActionName.Text,
                            Delay_MS = Int32.Parse(CheckStringToInt(Delay.Text)),
                            X = Int32.Parse(CheckStringToInt(MousePointX.Text)),
                            Y = Int32.Parse(CheckStringToInt(MousePointY.Text))
                        };
                        break;
                    case "前滾":
                    case "後滾":
                        Action = new MouseAction()
                        {
                            Tool_Name = "滑鼠",
                            Action_Desc = ActionName.Text,
                            Delay_MS = Int32.Parse(CheckStringToInt(Delay.Text)),
                            X = Int32.Parse(CheckStringToInt(MouseWheel.Text))
                        };
                        break;
                }
            }
        }

        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 回傳鍵盤動作
        /// </summary>
        /// <returns></returns>
        public KeyCodeAction GetKeyCodeAction()
        {
            return (KeyCodeAction)Action;
        }

        /// <summary>
        /// 回傳滑鼠動作
        /// </summary>
        /// <returns></returns>
        public MouseAction GetMouseAction()
        {
            return (MouseAction)Action;
        }

        /// <summary>
        /// 回傳鍵盤設定值
        /// </summary>
        /// <returns></returns>
        public string GetKeyCode(KeyCodeAction key)
        {
            return key.KeyCode;
        }

        /// <summary>
        /// 回傳滑鼠設定座標
        /// </summary>
        /// <returns></returns>
        public string GetPoint(MouseAction mouse)
        {
            return $"{mouse.X},{mouse.Y}";
        }

        /// <summary>
        /// 檢查字串是否為空或非數字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CheckStringToInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || Regex.IsMatch(input, @"\D"))
                return "0";
            else
                return input;
        }

        #region 顯示、隱藏

        /// <summary>
        /// 顯示鍵盤相關物件
        /// </summary>
        private void ShowKeyCode()
        {
            KeyCode.Visible = true;
            KeyCodeLabel.Visible = true;
        }

        /// <summary>
        /// 隱藏鍵盤相關物件
        /// </summary>
        private void HideKeyCode()
        {
            KeyCode.Visible = false;
            KeyCodeLabel.Visible = false;
        }

        /// <summary>
        /// 隱藏滑鼠相關物件
        /// </summary>
        private void HideMouse()
        {
            HideMouseClick();
            HideMouseRoll();
        }

        /// <summary>
        /// 顯示滑鼠點擊相關物件
        /// </summary>
        private void ShowMouseClick()
        {
            MousePointX.Visible = true;
            MousePointY.Visible = true;
            MousePointLabel.Visible = true;
            SetMousePoint.Visible = true;
        }

        /// <summary>
        /// 隱藏滑鼠點擊相關物件
        /// </summary>
        private void HideMouseClick()
        {
            MousePointX.Visible = false;
            MousePointY.Visible = false;
            MousePointLabel.Visible = false;
            SetMousePoint.Visible = false;
        }

        /// <summary>
        /// 顯示滑鼠滾動相關物件
        /// </summary>
        private void ShowMouseRoll()
        {
            MouseWheel.Visible = true;
            MouseWheelLabel.Visible = true;
        }

        /// <summary>
        /// 隱藏滑鼠滾動相關物件
        /// </summary>
        private void HideMouseRoll()
        {
            MouseWheel.Visible = false;
            MouseWheelLabel.Visible = false;
        }

        #endregion

        /// <summary>
        /// 選擇滑鼠座標事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetMousePoint_Click(object sender, EventArgs e)
        {
            BackGround b = new BackGround();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (b.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                MousePointX.Text = $"{b.Point.X}";
                MousePointY.Text = $"{b.Point.Y}";
            }

            b.Dispose();
        }

        /// <summary>
        /// 選擇滑鼠動作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tool.Text == "滑鼠")
            {
                switch (ActionName.Text)
                {
                    case "點一下左鍵":
                    case "點兩下左鍵":
                    case "點一下右鍵":
                    case "點一下中鍵":
                        HideMouseRoll();
                        ShowMouseClick();
                        break;
                    case "前滾": 
                    case "後滾":
                        HideMouseClick();
                        ShowMouseRoll();
                        break;
                }
            }
        }
    }
}
