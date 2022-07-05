using System.Threading;
using System.Runtime.InteropServices;
using System;

namespace AutoClickTest
{
    /// <summary>
    /// 滑鼠控制類別
    /// </summary>
    public static class Mouse
    {
        /// <summary>
        /// 滑鼠事件
        /// </summary>
        /// <param name="dwFlags">滑鼠按鈕</param>
        /// <param name="dx">X 座標</param>
        /// <param name="dy">Y 座標</param>
        /// <param name="dwData">動量</param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("User32")]
        public static extern void mouse_event(
            int dwFlags,
            int dx,
            int dy,
            int dwData,
            int dwExtraInfo
        );

        /// <summary>
        /// 設定鼠標位置
        /// </summary>
        /// <param name="x">X 座標</param>
        /// <param name="y">Y 座標</param>
        [DllImport("User32.dll")]
        public extern static void SetCursorPos(int x, int y);


        const int MOUSE_ABSOLUTE = 0x8000;
        const int MOUSE_LEFTDOWN = 0x0002;
        const int MOUSE_LEFTUP = 0x0004;
        const int MOUSE_MIDDLEDOWN = 0x0020;
        const int MOUSE_MIDDLEUP = 0x0040;
        const int MOUSE_MOVE = 0x0001;
        const int MOUSE_RIGHTDOWN = 0x0008;
        const int MOUSE_RIGHTUP = 0x0010;
        const int MOUSE_WHEEL = 0x0800;
        const int MOUSE_XDOWN = 0x0080;
        const int MOUSE_XUP = 0x1000;
        const int MOUSE_HWHEEL = 0x01000;

        /// <summary>
        /// 滑鼠中鍵滾動方向
        /// </summary>
        public enum ROLLWAY : int
        {
            FRONT = 120,
            BACK = -120
        }
        
        /// <summary>
        /// 滑鼠滾動事件
        /// </summary>
        /// <param name="way">滾動方向</param>
        public static void Roll(ROLLWAY way)
        {
            mouse_event(MOUSE_WHEEL , 0, 0, (int)way, 0);
        }

        /// <summary>
        /// 滑鼠左鍵點擊事件
        /// </summary>
        public static void LeftClick()
        {
            mouse_event(MOUSE_LEFTDOWN | MOUSE_LEFTUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 滑鼠右鍵點擊事件
        /// </summary>
        public static void RightClick()
        {
            mouse_event(MOUSE_RIGHTDOWN | MOUSE_RIGHTUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 滑鼠中鍵點擊事件
        /// </summary>
        public static void MiddleClick()
        {
            mouse_event(MOUSE_MIDDLEDOWN | MOUSE_MIDDLEUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 滑鼠移動事件
        /// </summary>
        /// <param name="x">X 座標</param>
        /// <param name="y">Y 座標</param>
        public static void Move(int x, int y)
        {
            SetCursorPos(x, y);
        }
    }
}
