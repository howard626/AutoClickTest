using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        }
        /// <summary>
        /// 增加排程動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAction(object sender, EventArgs e)
        {
            AddForm addDialog = new AddForm();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (addDialog.ShowDialog(this) == DialogResult.OK)
            {
                addDialog.Action.ID = ActionCount++;
                Actions.Add(addDialog.Action);
                Type type = addDialog.Action.GetType();
                if (type == typeof(KeyCodeAction))
                {
                    KeyCodeAction key = addDialog.GetKeyCodeAction();
                    ActionList.Nodes.Add($"{key.Action_Desc}{addDialog.GetKeyCode(key)}鍵--延遲 {key.Delay_MS} 秒");
                }
                else if (type == typeof(MouseAction))
                {
                    MouseAction mouse = addDialog.GetMouseAction();
                    switch (mouse.Action_Desc)
                    {
                        case "點一下左鍵":
                        case "點兩下左鍵":
                        case "點一下右鍵":
                        case "點一下中鍵":
                            ActionList.Nodes.Add($"在 {addDialog.GetPoint(mouse)} {mouse.Action_Desc}--延遲 {mouse.Delay_MS} 秒");
                            break;
                        case "前滾":
                        case "後滾":
                            ActionList.Nodes.Add($"滑鼠{mouse.Action_Desc} {mouse.X} 次--延遲 {mouse.Delay_MS} 秒");
                            break;
                        default:
                            ActionList.Nodes.Add($"在 {addDialog.GetPoint(mouse)} {mouse.Action_Desc}--延遲 {mouse.Delay_MS} 秒");
                            break;
                    }
                    
                }
                txtResult.Text = "Ok";
            }
            else
            {
                txtResult.Text = "Cancelled";
            }
            addDialog.Dispose();
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
        private void Run()
        {
            int delaySec = 0;
            for (int i = 0; i < Actions.Count; i++)
            {
                delaySec += Actions[i].Delay_MS;
                if (i == Actions.Count - 1)
                    Do(Actions[i], delaySec, Actions[i].GetType(), true); 
                else
                    Do(Actions[i], delaySec, Actions[i].GetType());
            }
        }

        /// <summary>
        /// 開始執行動作
        /// </summary>
        private async void Do(Action action, int delaySec, Type type, bool end = false)
        {
            Random random = new Random();
            await Task.Delay(delaySec + random.Next(-100, 100));
            if (IsStart)
            {
                if (type == typeof(KeyCodeAction))
                {
                    KeyCodeAction key = (KeyCodeAction)action;
                    switch (key.Action_Desc)
                    {
                        case "按一下":
                            Keybord.Press(key.KeyCode.ToUpper());
                            break;
                        case "按住":
                            Keybord.Hold(key.KeyCode.ToUpper(), 5000);
                            break;
                        default:
                            break;
                    }
                }
                else if (type == typeof(MouseAction))
                {
                    MouseAction mouse = (MouseAction)action;
                    switch (mouse.Action_Desc)
                    {
                        case "點一下左鍵":
                            Mouse.Move(mouse.X + random.Next(-10, 10), mouse.Y + random.Next(-10, 10));
                            Mouse.LeftClick();
                            break;
                        case "點兩下左鍵":
                            Mouse.Move(mouse.X + random.Next(-10, 10), mouse.Y + random.Next(-10, 10));
                            Mouse.LeftClick();
                            Thread.Sleep(50);
                            Mouse.LeftClick();
                            break;
                        case "點一下右鍵":
                            Mouse.Move(mouse.X + random.Next(-10, 10), mouse.Y + random.Next(-10, 10));
                            Mouse.RightClick();
                            break;
                        case "點一下中鍵":
                            Mouse.Move(mouse.X + random.Next(-10, 10), mouse.Y + random.Next(-10, 10));
                            Mouse.MiddleClick();
                            break;
                        case "前滾":
                            for (int j = 0; j < mouse.X; j++)
                            {
                                Mouse.Roll(Mouse.ROLLWAY.FRONT);
                            }
                            break;
                        case "後滾":
                            for (int j = 0; j < mouse.X; j++)
                            {
                                Mouse.Roll(Mouse.ROLLWAY.BACK);
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (end && IsStart)
                {
                    Run();
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

        /// <summary>
        /// 清除按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, EventArgs e)
        {
            IsStart = false;
            Actions.Clear();
            ActionList.Nodes.Clear();
        }
    }
}
