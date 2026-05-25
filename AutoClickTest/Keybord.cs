using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClickTest
{
    public static class Keybord
    {
        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_SCANCODE = 0x0008;

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public INPUTUNION u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUTUNION
        {
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, [In] INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private static void SendScanCode(ushort scanCode, uint flags)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = 0;
            inputs[0].u.ki.wScan = scanCode;
            inputs[0].u.ki.dwFlags = flags | KEYEVENTF_SCANCODE;
            inputs[0].u.ki.time = 0;
            inputs[0].u.ki.dwExtraInfo = IntPtr.Zero;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>按一下（按下 + 放開）</summary>
        public static void Press(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            ushort scan = (ushort)MapVirtualKey(vk, 0);
            SendScanCode(scan, 0);
            Thread.Sleep(20);
            SendScanCode(scan, KEYEVENTF_KEYUP);
        }

        /// <summary>只按下（不放開），用於組合鍵</summary>
        public static void PressDown(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            ushort scan = (ushort)MapVirtualKey(vk, 0);
            SendScanCode(scan, 0);
        }

        /// <summary>只放開，用於組合鍵</summary>
        public static void PressUp(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            ushort scan = (ushort)MapVirtualKey(vk, 0);
            SendScanCode(scan, KEYEVENTF_KEYUP);
        }

        /// <summary>按住指定毫秒後放開</summary>
        public static void Hold(string keyName, int ms)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            ushort scan = (ushort)MapVirtualKey(vk, 0);
            SendScanCode(scan, 0);
            Thread.Sleep(ms);
            SendScanCode(scan, KEYEVENTF_KEYUP);
        }
    }
}
