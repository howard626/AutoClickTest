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
        /// 是否執行 (legacy flag still present for a quick check)
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

        private ActionExecutor _executor;

        public AutoClick()
        {
            InitializeComponent();
        }

        private void AutoClick_Load(object sender, EventArgs e)
        {

        }

        private void AddAction(object sender, EventArgs e)
        {
            AddForm addDialog = new AddForm();

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

        private void Start_Click(object sender, EventArgs e)
        {
            if (Actions.Count == 0)
            {
                MessageBox.Show("請先新增動作");
                return;
            }

            IsStart = true;

            // create executor if null
            if (_executor == null)
            {
                _executor = new ActionExecutor(Actions);
                _executor.OnStatus += (s) => { this.BeginInvoke((Action)(() => { txtResult.Text = s; })); };
            }

            // read repeat count from UI if present
            try
            {
                _executor.SequenceRepeat = (int)numericUpDownRepeat.Value;
            }
            catch { _executor.SequenceRepeat = 1; }

            _executor.Start();
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            _executor?.Pause();
        }

        private void Resume_Click(object sender, EventArgs e)
        {
            _executor?.Resume();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            IsStart = false;
            _executor?.Stop();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            BackGround b = new BackGround();

            if (b.ShowDialog(this) == DialogResult.OK)
            {
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

        private void Clear_Click(object sender, EventArgs e)
        {
            IsStart = false;
            Actions.Clear();
            ActionList.Nodes.Clear();
            _executor?.Stop();
            _executor = null;
        }
    }
}
