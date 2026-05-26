using System;
using System.Runtime.InteropServices;

namespace AutoClickTest
{
    public static class Mouse
    {
        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        static extern bool SetProcessDPIAware();

        // 系統指標常量
        const int SM_XVIRTUALSCREEN = 76;   // 虛擬桌面左邊界
        const int SM_YVIRTUALSCREEN = 77;   // 虛擬桌面上邊界
        const int SM_CXVIRTUALSCREEN = 78;  // 虛擬桌面總寬度
        const int SM_CYVIRTUALSCREEN = 79;  // 虛擬桌面總高度

        const int INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        const uint MOUSEEVENTF_WHEEL = 0x0800;
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        const uint MOUSEEVENTF_VIRTUALDESK = 0x4000; // 必須包含此標記以支援多螢幕

        public enum ROLLWAY : int
        {
            FRONT = 120,
            BACK = -120
        }

        static Mouse()
        {
            SetProcessDPIAware();
        }

        /// <summary>
        /// 修正後的移動方法：支援多螢幕與虛擬座標轉換
        /// </summary>
        public static void Move(int x, int y)
        {
            // 獲取整個虛擬桌面的邊界資訊
            int vLeft = GetSystemMetrics(SM_XVIRTUALSCREEN);
            int vTop = GetSystemMetrics(SM_YVIRTUALSCREEN);
            int vWidth = GetSystemMetrics(SM_CXVIRTUALSCREEN);
            int vHeight = GetSystemMetrics(SM_CYVIRTUALSCREEN);

            // 在多螢幕系統下，ABSOLUTE 座標計算公式如下：
            // 座標 = (目標位置 - 虛擬桌面起始位置) * 65535 / 虛擬桌面總尺寸
            // 注意：x, y 若是從 Control.MousePosition 抓到的，已經包含了負值
            int normalizedX = ((x - vLeft) * 65536) / vWidth;
            int normalizedY = ((y - vTop) * 65536) / vHeight;

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dx = normalizedX;
            inputs[0].mi.dy = normalizedY;
            // 關鍵：必須同時使用 ABSOLUTE 和 VIRTUALDESK
            inputs[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_VIRTUALDESK;
            inputs[0].mi.time = 0;
            inputs[0].mi.dwExtraInfo = IntPtr.Zero;

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void LeftClick()
        {
            INPUT[] inputs = new INPUT[2];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;
            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void RightClick()
        {
            INPUT[] inputs = new INPUT[2];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dwFlags = MOUSEEVENTF_RIGHTUP;
            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void MiddleClick()
        {
            INPUT[] inputs = new INPUT[2];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dwFlags = MOUSEEVENTF_MIDDLEDOWN;
            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dwFlags = MOUSEEVENTF_MIDDLEUP;
            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void Roll(ROLLWAY way)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dwFlags = MOUSEEVENTF_WHEEL;
            inputs[0].mi.mouseData = (uint)way;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 移動並點擊（確保移動到位置後才觸發點擊）
        /// </summary>
        public static void MoveAndLeftClick(int x, int y)
        {
            int vLeft = GetSystemMetrics(SM_XVIRTUALSCREEN);
            int vTop = GetSystemMetrics(SM_YVIRTUALSCREEN);
            int vWidth = GetSystemMetrics(SM_CXVIRTUALSCREEN);
            int vHeight = GetSystemMetrics(SM_CYVIRTUALSCREEN);

            int nx = ((x - vLeft) * 65536) / vWidth;
            int ny = ((y - vTop) * 65536) / vHeight;

            INPUT[] inputs = new INPUT[3];

            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dx = nx;
            inputs[0].mi.dy = ny;
            inputs[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_VIRTUALDESK;

            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;

            inputs[2].type = INPUT_MOUSE;
            inputs[2].mi.dwFlags = MOUSEEVENTF_LEFTUP;

            SendInput(3, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}