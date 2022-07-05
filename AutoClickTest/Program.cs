using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClickTest
{
    static class Program
    {
        

        /// <summary>
        /// 設置鼠標位置
        /// </summary>
        /// <param name="x">X 座標 左上角為(0,0)</param>
        /// <param name="y">Y 座標 左上角為(0,0)</param>
        [DllImport("User32.dll")]
        public extern static void SetCursorPos(int x, int y);//設置鼠標位置

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AutoClick());
        }
    }
}
