using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClickTest
{
    public static class Keybord
    {
        // ---------------------------------------------------------
        // Win32 API 結構與常數定義
        // ---------------------------------------------------------

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

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
        struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct INPUTUNION
        {
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public INPUTUNION u;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern ushort MapVirtualKey(uint uCode, uint uMapType);

        private const int INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_SCANCODE = 0x0008;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

        // ---------------------------------------------------------
        // 私有核心發送方法
        // ---------------------------------------------------------

        private static void SendInputKey(ushort scanCode, bool isKeyUp)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = 0; // 設為 0 使用 ScanCode 模式，避開遊戲偵測
            inputs[0].u.ki.wScan = scanCode;

            // 設定 Flags：必帶 SCANCODE，如果是放開則加畫 KEYUP
            uint flags = KEYEVENTF_SCANCODE;
            if (isKeyUp) flags |= KEYEVENTF_KEYUP;

            // 處理擴充鍵 (如方向鍵、Delete等)，雖然 Space 不需要，但為了完整性加上
            if ((scanCode & 0xFF00) == 0xE000) flags |= KEYEVENTF_EXTENDEDKEY;

            inputs[0].u.ki.dwFlags = flags;
            inputs[0].u.ki.time = 0;
            inputs[0].u.ki.dwExtraInfo = IntPtr.Zero;

            // 執行發送
            uint result = SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));

            if (result == 0)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine($"SendInput 失敗，錯誤代碼: {error}");
            }
        }

        // ---------------------------------------------------------
        // 公開呼叫方法
        // ---------------------------------------------------------

        /// <summary>
        /// 按一下（按下 + 延遲 + 放開）
        /// </summary>
        public static void Press(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            ushort scan = MapVirtualKey((uint)key, 0);

            SendInputKey(scan, false); // 按下
            Thread.Sleep(80);          // 模擬人類反應時間 (重要：太短遊戲會過濾)
            SendInputKey(scan, true);  // 放開
        }

        /// <summary>
        /// 只按下不放開（用於組合鍵或特殊操作）
        /// </summary>
        public static void PressDown(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            ushort scan = MapVirtualKey((uint)key, 0);
            SendInputKey(scan, false);
        }

        /// <summary>
        /// 只放開按鍵
        /// </summary>
        public static void PressUp(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            ushort scan = MapVirtualKey((uint)key, 0);
            SendInputKey(scan, true);
        }

        /// <summary>
        /// 按住指定毫秒後放開
        /// </summary>
        public static void Hold(string keyName, int ms)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            ushort scan = MapVirtualKey((uint)key, 0);

            SendInputKey(scan, false);
            Thread.Sleep(ms);
            SendInputKey(scan, true);
        }
    }
}