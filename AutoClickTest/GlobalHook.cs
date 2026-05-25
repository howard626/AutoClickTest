using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoClickTest
{
    public class GlobalHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;

        /// <summary>按住超過此毫秒數即記錄為「按住」動作</summary>
        public static int HoldThresholdMs = 150;

        /// <summary>目前正在被按住的鍵，以及它們被按下的時間（排除已釋放的）</summary>
        public static Dictionary<Keys, DateTime> CurrentlyHeldKeys
        {
            get { lock (_keyDownTimes) { return new Dictionary<Keys, DateTime>(_keyDownTimes); } }
        }

        public static bool IsModifier(Keys key)
        {
            return key == Keys.LShiftKey || key == Keys.RShiftKey || key == Keys.ShiftKey
                || key == Keys.LControlKey || key == Keys.RControlKey || key == Keys.ControlKey
                || key == Keys.LMenu || key == Keys.RMenu || key == Keys.Menu
                || key == Keys.LWin || key == Keys.RWin;
        }

        private static LowLevelHookProc _procKeyboard = KeyboardHookCallback;
        private static LowLevelHookProc _procMouse = MouseHookCallback;
        private static IntPtr _hookIDKeyboard = IntPtr.Zero;
        private static IntPtr _hookIDMouse = IntPtr.Zero;

        // 記錄每個按鍵的按下時間
        private static readonly Dictionary<Keys, DateTime> _keyDownTimes = new Dictionary<Keys, DateTime>();

        /// <summary>觸發時機：按鍵放開。int = 按住毫秒數</summary>
        public static event Action<Keys, int> OnKeyEvent;
        public static event Action<int, int, string> OnMouseClick; // X, Y, ButtonType

        public static void Start()
        {
            _keyDownTimes.Clear();
            _hookIDKeyboard = SetHook(_procKeyboard, WH_KEYBOARD_LL);
            _hookIDMouse = SetHook(_procMouse, WH_MOUSE_LL);
        }

        public static void Stop()
        {
            UnhookWindowsHookEx(_hookIDKeyboard);
            UnhookWindowsHookEx(_hookIDMouse);
            _hookIDKeyboard = IntPtr.Zero;
            _hookIDMouse = IntPtr.Zero;
            _keyDownTimes.Clear();
        }

        public static bool IsRecording => _hookIDKeyboard != IntPtr.Zero;

        private static IntPtr SetHook(LowLevelHookProc proc, int hookType)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(hookType, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                bool isDown = wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN;
                bool isUp   = wParam == (IntPtr)WM_KEYUP   || wParam == (IntPtr)WM_SYSKEYUP;

                if (isDown)
                {
                    // 只記錄第一次按下（忽略 auto-repeat）
                    if (!_keyDownTimes.ContainsKey(key))
                        _keyDownTimes[key] = DateTime.Now;
                }
                else if (isUp)
                {
                    if (_keyDownTimes.TryGetValue(key, out DateTime downTime))
                    {
                        int holdMs = (int)(DateTime.Now - downTime).TotalMilliseconds;
                        _keyDownTimes.Remove(key);
                        OnKeyEvent?.Invoke(key, holdMs);
                    }
                }
            }
            return CallNextHookEx(_hookIDKeyboard, nCode, wParam, lParam);
        }

        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                string btn = "";
                if (wParam == (IntPtr)WM_LBUTTONDOWN) btn = "Left";
                else if (wParam == (IntPtr)WM_RBUTTONDOWN) btn = "Right";
                else if (wParam == (IntPtr)WM_MBUTTONDOWN) btn = "Middle";

                if (!string.IsNullOrEmpty(btn))
                    OnMouseClick?.Invoke(hookStruct.pt.x, hookStruct.pt.y, btn);
            }
            return CallNextHookEx(_hookIDMouse, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int x; public int y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData, flags, time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
