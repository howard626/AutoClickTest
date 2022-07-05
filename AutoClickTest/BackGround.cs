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

        private void BackGround_Load(object sender, EventArgs e)
        {
            MouseClick += new MouseEventHandler(Get_MousePointClick);
        }

        /// <summary>
        /// 滑鼠點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Get_MousePointClick(Object sender, MouseEventArgs e)
        {
            GetCursorPos(out Point point);
            Point = point;
            MouseClick -= Get_MousePointClick;
            this.DialogResult = DialogResult.OK;
        }

        public Point Point { get; set; }
    }
}
